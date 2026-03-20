# Coding Style

## Formatting

- Tabs for indentation. One tab per nesting level.
- No variable name prefixes. Use `grid` not `_grid`. The only exception is constructor/method parameter disambiguation: `this.grid = grid`.
- Single-line body: keep `if`, `for`, `foreach` on one line without braces when the body is a single statement:
  ```csharp
  if (count > 0) return true;
  for (int i = 0; i < 10; i++) list.Add(i);
  foreach (var sq in squares) grid.Mark(sq);
  ```
- Multi-line body: always wrap with `{ }`:
  ```csharp
  if (count > 0)
  {
      DoFirst();
      DoSecond();
  }
  ```
- Keep signatures on one line when they fit. Do not split lambda expressions or method signatures across multiple lines unless the line would exceed ~120 characters.
- Prefer expression-bodied members for single-expression properties and methods: `public int Count => list.Count;`

## Comments

- Use only `/// <summary>` XML doc comments. No `<param>`, `<returns>`, `<remarks>` unless truly needed.
- Use ASCII characters in comments. Write `->` not `→`, `-` not `—` or `–`.
- No inline `//` comments except to label a non-obvious line.

## Naming

- The game is **Battleship**. The opponent is **Computer**
- PascalCase for types, methods, properties, events. camelCase for locals, parameters, private fields.
- No Hungarian notation. No `_` prefixes.

## Structure

- Organise each class with `#region` blocks: Fields, Constructor, Public Methods, Private Methods, Helpers.
- One type per file. Enum may live in the same file as the class that uses it most (e.g. `CellState` in `SquareCellViewModel.cs`, `HitResult` in `Fleet.cs`).
- `Model` project: no UI references. `GUI` project: depends on `Model`. `UnitTests` depends on `Model` only.
- Views live in `GUI/Views/`. ViewModels live in `GUI/ViewModels/`. The `ViewLocator` maps ViewModel types to View types by replacing "ViewModel" with "View" in the full type name.

## Testing

- MSTest via `Microsoft.NET.Test.Sdk`. Test classes named `Test{ClassName}`.
- All tests must pass before commit. Run: `dotnet test`.
