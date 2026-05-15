# Refinements And Changes

## Development Log
- Milestone 1: created the Unity project structure and initial documentation set.
- Milestone 2: built the first menu and panel flow for a text-first detective prototype.
- Milestone 3: added case data classes, suspect cards, and a fallback case briefing flow.
- Milestone 4: added Ollama HTTP integration and early generated case support.
- Milestone 5: tested live suspect interrogation and found that repeated runtime calls were too fragile for the assignment timeline.
- Milestone 6: rebuilt the project around one structured Ollama request per case.
- Milestone 7: updated the UI flow so each suspect now has an opening statement plus two generated follow-up questions.
- Milestone 8: improved fallback behavior, menu presentation, and assignment documentation.

## Scope Changes
- The original free-text interrogation concept was reduced to a safer preset-question format.
- This was the biggest design change in the project and was made to improve stability, demo reliability, and clarity.

## AI-Assisted Decisions
- Chose a small, UI-first vertical slice instead of a larger exploratory game.
- Chose to move from repeated live inference to one structured generation request per case.
- Chose to add structured JSON schema output to reduce parsing failures.
- Chose to keep fallback cases so the project remains demoable even if the local model fails.

## Bugs Encountered
- Invalid or incomplete JSON responses from Ollama.
- Repeated fallback use when the generated response did not match the parser.
- UI layering issues on the start menu background.
- Mismatch between generated character gender and fixed portrait order.

## Fixes Made
- Reworked the prompt to generate the entire round in one response.
- Added structured output with a JSON schema.
- Added clearer parsing rules and fallback handling.
- Added multiple fallback cases instead of a single repeated case.
- Forced the victim and suspect gender order to match the available portraits.
- Repaired scene UI settings so the background art displays correctly.

## Prompt Improvements
- Moved from a plain JSON prompt to a schema-backed structured output request.
- Added exact suspect count and exact follow-up question count.
- Added tone constraints and clue-solving rules.
- Added a guard against repeating the previous case too closely.

## Design Changes
- The game remains one scene and text-first.
- The core LLM feature now focuses on generated narrative packages rather than live improvisation.
- This keeps the deduction experience while reducing failure points.

## Technical Changes
- Added `OllamaClient` for local HTTP requests.
- Added `PromptBuilder` for the final structured generation prompt and schema.
- Reworked `GameManager` to run one generation request per round.
- Added `FollowUpQuestion` data support.
- Updated suspect data to include opening statements and generated follow-up content.
- Replaced the old fallback dialogue system with fallback full-case data.

## What Was Simplified And Why
- Unlimited typed interrogation was removed because it introduced too much instability for a student prototype.
- The final version still satisfies the assignment by making Ollama responsible for core gameplay content.
- Simplifying the interaction model made the build easier to test, explain, and demonstrate professionally.
