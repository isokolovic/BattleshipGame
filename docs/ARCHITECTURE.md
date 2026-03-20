# Architecture

## Overview

Three projects with strict layering. `Model` has no UI dependencies. `GUI` depends on `Model`. Tests depend on `Model` only.

```
GUI  ->  Model
Tests ->  Model
```

## Model

All game logic. No Avalonia, no UI references.

### Grid hierarchy

```
Grid (abstract)
├── FleetGrid     ship placement
└── EnemyGrid     computer shot tracking
```

`Grid` holds a 2D `Square[,]` array and provides `GetAvailablePlacements(length)` - returns all horizontal and vertical runs of available squares that fit a ship of the given length. Both `FleetGrid` and `EnemyGrid` inherit this but define "available" differently.

`FleetGrid` - squares can be nulled out via `EliminateSquare`. Used by `Shipwright` to block the exclusion zone around each placed ship.

`EnemyGrid` - squares have a `SquareState` that can only advance forward (`Initial -> Eliminated -> Missed -> Hit -> Sunken`). Supports directional scans via `GetAvailableSquares(row, col, direction)`.

### Square

One cell on any grid. Identified by `Row` and `Column`. Equality is coordinate-based only. State can only advance forward; `ChangeState` ignores backwards transitions.

```
SquareState order (do not reorder - numeric values are compared directly):
Initial -> Eliminated -> Missed -> Hit -> Sunken
```

### Ship and Fleet

`Ship` holds an `IEnumerable<Square>` and handles hit detection. When all squares are hit, it marks them all `Sunken` and returns `HitResult.Sunken`.

`Fleet` is a list of ships. `Shoot` delegates to each ship in order and returns the first non-`Missed` result.

### Shipwright

Randomly places ships one by one onto a `FleetGrid`. After placing each ship it calls `SquareEliminator.ToEliminate` to get the padded bounding box and nulls those squares out (no-adjacency rule).

If placement fails (no room), it returns an empty `Fleet`. The caller must retry - `BattleShip.cs` in the GUI project handles this with a `do/while` loop.

### SquareEliminator

Given a list of ship squares, returns all squares in the bounding rectangle padded by 1 cell in every direction, clamped to grid bounds.

### LimitedQueue

A `Queue<T>` that evicts the oldest item before enqueuing when at capacity. Used by `Grid.GetPlacements` as a sliding window.

## GUI

Avalonia MVVM project. Depends on `Model`. No code-behind logic except `PointerPressed` event forwarding in `GridView`.

### ViewModel hierarchy

```
MainWindowViewModel
└── GameViewModel
    ├── GridViewModel (PlayerGrid)
    │   └── SquareCellViewModel x100
    └── GridViewModel (EnemyGrid)
        └── SquareCellViewModel x100
```

`SquareCellViewModel` - one cell. Holds `CellState`, computes `BackgroundBrush` and `IsClickable`. Enemy cells get an `Action<SquareCellViewModel>` callback; clicking fires `ShootCommand`.

`GridViewModel` - 100 cells in a flat `ObservableCollection` in row-major order (`index = row * 10 + col`). The `ItemsControl` uses a `UniformGrid` panel.

`GameViewModel` - owns a `BattleShip` facade, both `GridViewModel` instances, all game state. Handles the full game loop including the async computer turn with a 300ms delay. Manages `IsPlayerGridActive` / `IsEnemyGridActive` to highlight the active player's grid.

`MainWindowViewModel` - top-level. Controls start/game screen switching via `IsGameStarted`. Holds the single `GameViewModel` instance reused across restarts.

### BattleShip facade

`GUI/BattleShip.cs` wraps the Model layer so `GameViewModel` does not talk to `Gunnery`, `Shipwright`, and both `Fleet` instances directly. Owns the retry loop for fleet placement and exposes a nullable `Square?` from `GetComputerTarget`.

## Key design decisions

| Decision | Reason |
|---|---|
| `Square.State` not `Square.SquareState` | Avoids naming collision between property and enum type |
| `HashCode.Combine` in `GetHashCode` | XOR has collision patterns, e.g. (1,2) == (2,1) |
| `SquareEliminator` uses `Min`/`Max` | Input order of ship squares is not guaranteed |
| `InlineShooting` guards both-ends-blocked | Prevents `InvalidOperationException` on `.First()` when both ends are blocked |
| `RandomShooting` returns `null` when no placements | Prevents crash on `.ElementAt` on empty sequence |
| `Gunnery.ChangeToRandomTactics` guards empty `shipsToShoot` | After last ship sunk the list is empty; game is over |
