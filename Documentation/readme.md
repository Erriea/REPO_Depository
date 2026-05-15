# Project README

## Project Overview
- `CaseFile: Local Suspect` is a Unity detective prototype with local Ollama integration.
- Each round generates a crime, a victim, three suspects, two follow-up questions per suspect, a culprit, and a final explanation.

## Final Gameplay Flow
1. Open the main menu.
2. Press `New Case`.
3. Let Ollama generate a structured detective case.
4. Read the case briefing.
5. Select suspects and read their opening statements.
6. Ask the two generated follow-up questions for each suspect.
7. Accuse one suspect.
8. Review the clue and explanation on the result screen.

## How To Run The Game
1. Open the Unity project.
2. Start Ollama locally.
3. Make sure the model `llama3.2` is available.
4. Open `Assets/Scenes/MainScene.unity`.
5. Press Play.

## Dependencies
- Unity `6000.0.44f1`
- Ollama
- A local model such as `llama3.2`

## Controls
- Mouse for menu navigation and suspect selection.
- Keyboard for entering `1` or `2` in the interrogation input box to choose a suspect’s generated follow-up question.

## LLM Feature Summary
- Unity sends one local HTTP request to Ollama per new case.
- Ollama returns structured JSON containing the full round data.
- Unity parses the result and uses it to populate the UI and deduction loop.

## Fallback Behaviour
- If Ollama is unavailable or returns invalid data, the game loads a prepared fallback case instead.
- Multiple fallback cases are included so the failure mode is still playable.

## Known Limitations
- The first Ollama request may be slow while the model warms up.
- Local models may still produce repetitive or weaker cases.
- The current final design uses generated preset questions instead of unlimited free-text interrogation.

## Project Documentation
- `high-concept.md`
- `ollama-plan.md`
- `setup.md`
- `prompt-archive.md`
- `prompts-used.md`
- `llm-integration-report.md`
- `refinements-changes.md`
- `assignment-checklist.md`

## AI Tools Used
- Ollama for local LLM inference
- Codex for development assistance, refactoring support, and documentation drafting

## Credits
- Custom background art and project assets supplied by the project owner unless otherwise noted in the repository.

## Licensing Notes
- Ollama and the selected local model must be used according to their own licenses.
- The project should clearly state in the final submission that AI-generated narrative content is used in gameplay.
