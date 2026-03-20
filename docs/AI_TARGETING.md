# Computer Targeting

The computer uses a three-phase targeting strategy managed by `Gunnery`. Each phase is a separate class implementing `INextTarget`.

## Phases

```
Random -> first hit -> Surrounding -> second hit -> Inline
                                                       │
                       <---------- ship sunk ----------┘
```

### Phase 1 - Random (`RandomShooting`)

No active hit sequence. Picks a random square from all squares that are part of a valid placement for the smallest remaining ship. Never shoots at squares that cannot contain any remaining ship.

### Phase 2 - Surrounding (`SurroundingShooting`)

Triggered after the first hit. Probes the four cardinal neighbours of the first hit square, skipping already-marked ones. Returns `null` if all four are taken - `Gunnery` handles null via the caller's guard.

### Phase 3 - Inline (`InlineShooting`)

Triggered after a second hit confirms orientation. Scans both open ends of the current hit sequence and shoots toward whichever end has more open space. Returns `null` if both ends are blocked.

## Tactic transitions

| Event | From | To |
|---|---|---|
| Miss | Any | Same (no change) |
| First hit | Random | Surrounding |
| Second hit | Surrounding | Inline |
| Any hit | Inline | Inline (stays) |
| Ship sunk | Any | Random |

## State tracking

`Gunnery` maintains:
- `monitoringGrid` - an `EnemyGrid` recording every shot result
- `squaresHit` - squares hit in the current unresolved sequence, cleared on sunk
- `shipsToShoot` - remaining ship lengths
- `lastTarget` - cached between `NextTarget()` and `ProcessHitResult()` calls

## Elimination zones

When a ship sinks, `Gunnery` calls `SquareEliminator.ToEliminate` on the sunk squares and marks the entire padded bounding rectangle as `Eliminated`. Ships cannot be adjacent so these squares are excluded from future targeting.
