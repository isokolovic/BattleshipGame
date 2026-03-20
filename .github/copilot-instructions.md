# Battleship - LLM Instructions

Read the topic-specific docs before making changes:

- [docs/ARCHITECTURE.md](../docs/ARCHITECTURE.md) - project structure, layer responsibilities, class hierarchy
- [docs/AI_TARGETING.md](../docs/AI_TARGETING.md) - computer targeting strategy (three-phase state machine)
- [docs/CODING_STYLE.md](../docs/CODING_STYLE.md) - formatting rules, naming, commenting conventions
- [docs/GUI_DESIGN.md](../docs/GUI_DESIGN.md) - Avalonia UI layout, theme, MVVM wiring, visual rules

## Quick reference

- Solution has three projects: `Model` (game logic, no UI), `GUI` (Avalonia MVVM), `UnitTests` (MSTest)
- `Model` knows nothing about `GUI`. `GUI` depends on `Model`. Tests depend on `Model`.
- Target framework: `net10.0`. Avalonia 11.x. 
- All tests must pass (`dotnet test`) before any PR or commit.
- Light theme. Clean, colorful, Material-style design.