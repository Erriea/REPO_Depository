# Refinements And Changes

## Development Log
- Milestone 1: project structure and documentation scaffolding.
- Milestone 2: added the first screen-management scripts for the main menu and placeholder panel flow.
- Milestone 3: added preview case data classes and case briefing UI scripts for the suspect card screen.
- Milestone 4: replaced the preview-only flow with a full fallback interrogation loop and added a one-click Unity scene builder for the custom art.

## Scope Changes
- None yet.

## AI-Assisted Decisions
- Chose a small, UI-first vertical slice to keep the prototype manageable.
- Prioritized a fully playable fallback version before Ollama integration so the project remains demoable even if local AI setup is delayed.

## Bugs Encountered
- None recorded yet.

## Fixes Made
- None recorded yet.

## Prompt Improvements
- Not started yet.

## Design Changes
- Not started yet.

## Technical Changes
- Began with a clean 2D Unity project layout and dedicated documentation files.
- Replaced the starter `Test.cs` script with a small game-state and UI-state foundation.
- Added beginner-friendly `CaseFile`, `Suspect`, and `CaseBriefingPanelUI` scripts so the case briefing screen can be tested before Ollama is connected.
- Imported custom portrait and background art into the project structure.
- Added a fallback case provider, dialogue generator, and panel controllers for briefing, interrogation, accusation, and result screens.
- Added an editor menu tool to generate and wire the main assignment scene automatically.

## What Was Simplified And Why
- The prototype is intentionally text-first with one main scene to reduce complexity for a beginner workflow.
