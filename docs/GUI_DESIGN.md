# GUI Design

## Theme

Light theme. Clean, colorful, Material-style. Think Google/Apple design language.

- `RequestedThemeVariant="Light"` in `App.axaml`.
- Window background: `#F5F5F5` (light gray).
- Grid card panels: white with `#E0E0E0` border, rounded corners (12px), minimal padding (`8,6`).
- Active player's grid gets a green border color (`#4CAF50`) via `Border.grid-panel.active` style class. No border thickness change - just color swap.

## Color palette

| State | Hex | Usage |
|---|---|---|
| Water (Initial) | `#90CAF9` | Light blue, unshot cells |
| Ship | `#546E7A` | Blue-gray, player's placed ships |
| Missed | `#CFD8DC` | Light gray-blue, missed shots |
| Hit | `#FF9800` | Orange, confirmed hit |
| Sunken | `#EF5350` | Red, destroyed ship |
| Eliminated | `#E0E0E0` | Light gray, impossible cells |

## Layout

### Start screen (`StartView`)
- Centered vertically. Anchor icon, "BATTLESHIP" title, rounded "Start Game" button.
- No subtitle. Just the title and the button.

### Game screen (`GameView`)
- **Top bar**: "New Game" button (left), game message (center, subtle), ship counts (right). Minimal padding.
- **Grid area**: two grid panels side by side, centered, minimal gap (12px) and margin (8,6). Grids must be adjacent - never put large UI elements between them.
- Active turn indicated by green border color on the active grid panel. No text overlays, no border thickness pop-out.
- "Computer goes first" message shown only at game start, clears after first turn.
- Game-over messages ("You win!", "Computer wins!") shown in the top bar center.

### Grid (`GridView`)
- 10x10 `UniformGrid` inside an `ItemsControl`. 36x36px cells, 1px margin, 4px corner radius.
- Column headers (A-J) and row numbers (1-10) in gray (`#757575`), `SemiBold`.
- Clickable cells (enemy, unshot) get `cursor: Hand` and opacity hover feedback via style classes.
- `PointerPressed` in code-behind forwards to `ShootCommand` after checking `IsClickable`.

## MVVM wiring

- `ViewLocator` maps ViewModel -> View by string replacement in the full type name.
- `CommunityToolkit.Mvvm` source generators: `[ObservableProperty]`, `[RelayCommand]`, `[NotifyPropertyChangedFor]`, `[NotifyCanExecuteChangedFor]`.
- No code-behind logic except the `PointerPressed` event in `GridView.axaml.cs`.
- `GameViewModel` properties `IsPlayerGridActive` / `IsEnemyGridActive` drive `Classes.active` on the grid panels.

## Rules

- Minimal padding everywhere. No unnecessary whitespace.
- Game title is "Battleship"