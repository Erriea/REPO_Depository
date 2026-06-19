# Project README

## Project Overview
- `CaseFile: Local Suspect` is a Unity detective game prototype powered by local Ollama case generation.
- Each round gives the player one case, three suspects, a set of linked clues, a limited interrogation phase, and one final arrest decision.
- The final version was refined directly around feedback on clarity, difficulty, presentation, and navigation.

## Final Gameplay Flow
1. Open the main menu.
2. Choose either `New Case` or `Timed Case`.
3. Wait for Ollama to generate a case, or let the game load a fallback case if generation fails.
4. Review the `Crime Board` summary.
5. Open `The Crime` to inspect the narrative setup, victim details, and on-site clues.
6. Open `The Suspects` and review one suspect at a time.
7. Open `Interrogation` and spend the `4` shared questions carefully across the suspects.
8. Unlock or reach the `Arrest` screen.
9. Select one suspect.
10. Read the `Outcome` screen to see whether the arrest was correct and which clues pointed to the culprit.

## Main Feedback Integrated
The main feedback we chose to focus on was:
- Giving the player clear key clues to solving the case.
- Giving a limited amount of questions to be asked.
- Adding a timer variant of gameplay for increased difficulty.
- Creating an updated and more navigable UI.

## How The Final Build Responds To That Feedback

### 1. Clearer Key Clues
- Cases are now structured so the player is given visible clues through the crime scene, suspect descriptions, and interrogation answers.
- The outcome screen explains which clues pointed to the culprit.
- Fallback cases were expanded so clue-based solving still works even without Ollama.

### 2. Limited Questions
- The old interrogation structure was replaced with `4` shared questions total.
- Each question can only be used once for the whole case.
- This means the player must decide which suspect is most worth questioning with each remaining prompt.

### 3. Timer Variant
- The main menu now includes a `Timed Case` option.
- Timed mode gives the player `1 minute` to review the available evidence.
- When time runs out, the player is forced to make an arrest based on what they managed to learn.

### 4. Updated Navigable UI
- The project was rebuilt into a clearer multi-panel structure:
  - `Main Menu`
  - `Crime Board`
  - `The Crime`
  - `The Suspects`
  - `Interrogation`
  - `Arrest`
  - `Outcome`
- The arrest button remains locked until the required investigation steps are completed in standard mode.
- The suspect review screen now shows one suspect at a time so the information stays readable.
- The game also includes visual loading feedback while waiting for Ollama.

## Additional Improvements Added
- Added fallback case content for the full gameplay loop, not just the summary screen.
- Added improved background presentation using the detective-board art across the interface.
- Added custom fonts for headings and body text.
- Added main menu music, mode-specific gameplay music, click sound effects, and result sounds.
- Added correct and incorrect outcome audio feedback.
- Added a visible timer for timed mode.
- Added safer Ollama failure messaging so the player understands when fallback content is being used.

## How To Run The Game
1. Open the Unity project.
2. Start Ollama locally.
3. Make sure the model `llama3.2` is installed.
4. Open `Assets/Scenes/MainScene.unity`.
5. Press Play.

## Dependencies
- Unity `6000.0.44f1`
- Ollama
- A local Ollama model such as `llama3.2`

## Controls
- Mouse for all menu navigation, panel navigation, suspect selection, question selection, and arrest confirmation.

## LLM Feature Summary
- Unity sends one structured local HTTP request to Ollama per new case.
- Ollama returns JSON containing:
  - case title
  - board summary
  - victim details
  - crime details
  - clues
  - suspects
  - interrogation questions and answers
  - guilty suspect
  - guilt clues
  - explanation
- Unity parses the response and uses it to populate the full deduction loop.

## Fallback Behaviour
- If Ollama is unavailable, slow, times out, or returns invalid case data, the game loads a fallback case.
- Fallback cases contain enough information to play through the full investigation properly.

## Known Limitations
- The first Ollama request may still be slower while the model warms up.
- Generated cases may vary in quality depending on the model and local performance.
- The project uses fixed portraits, so visual character representation is still more limited than a fully bespoke art pipeline.

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
- Codex for implementation support, UI rebuilding, debugging, audio/font integration, and documentation updates

## Credits
- Custom background art and project assets supplied by the project owner unless otherwise stated in the repository.
- Additional local audio and font files were imported into the build during refinement.

## Licensing Notes
- Ollama and the chosen local model must be used according to their own licenses.
- Imported fonts and audio assets should be credited and used according to their original license terms in the final submission if required.
