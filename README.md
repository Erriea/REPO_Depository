# CaseFile: Local Suspect

Unity detective game prototype with local Ollama-generated cases, a structured investigation flow, a timed challenge mode, updated UI presentation, and feedback-driven refinements.

## Documentation
- [Project README](C:/Code/2026%20Unity/REPO_Depository/Documentation/readme.md)
- [Refinements And Changes](C:/Code/2026%20Unity/REPO_Depository/Documentation/refinements-changes.md)
- [Setup Guide](C:/Code/2026%20Unity/REPO_Depository/Documentation/setup.md)
- [LLM Integration Report](C:/Code/2026%20Unity/REPO_Depository/Documentation/llm-integration-report.md)

## Core Features
- Ollama generates a full detective case in one structured request.
- The player investigates through `Crime Board`, `The Crime`, `The Suspects`, `Interrogation`, `Arrest`, and `Outcome` screens.
- Interrogation uses `4` shared questions total, so each question can only be spent once across all suspects.
- A `Timed Case` mode gives the player `1 minute` to gather evidence before they are forced into the arrest screen.
- Fallback cases are included so the game remains playable if Ollama is slow, unavailable, or returns unusable data.

## Extra Polish Added
- Updated multi-screen UI with clearer navigation and improved layout.
- Investigation-board background styling across the game screens.
- Custom fonts for headings and body text.
- Main menu and gameplay music.
- UI click sound effects and result sounds for correct and incorrect arrests.

## Run
1. Open the project in Unity `6000.0.44f1`.
2. Start Ollama locally and make sure `llama3.2` is installed.
3. Open `Assets/Scenes/MainScene.unity`.
4. Press Play.

For full setup and usage details, see [Documentation/setup.md](C:/Code/2026%20Unity/REPO_Depository/Documentation/setup.md).
