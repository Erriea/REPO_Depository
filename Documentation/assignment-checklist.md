# Assignment Checklist

## Checklist Purpose
- This file maps the assignment requirements to the current prototype and supporting documentation.

## Core Requirements
- [x] Defines LLMs and relevance to game design workflows
- [x] Differentiates hosted vs local LLMs
- [x] Demonstrates prompt engineering for narrative content
- [x] Produces branching or adaptive dialogue
- [x] Installs and configures a local LLM environment using Ollama
- [x] Generates case content using the LLM
- [x] Integrates LLM content into Unity
- [x] Compares output quality, latency, and control between local and cloud tools
- [x] Modifies generated text to match tone and style
- [x] Analyses limitations of LLM output
- [x] Develops a prototype with an LLM-powered feature
- [ ] Plans a technical demonstration video
- [ ] Plans a final showcase video
- [x] Completes the prompt archive
- [x] Completes the LLM integration report
- [x] Completes the README
- [x] Completes the setup instructions

## Evidence Locations
- LLM definition, relevance, player goal, and scope: `high-concept.md`
- Local vs hosted workflow comparison and local inference rationale: `ollama-plan.md`, `llm-integration-report.md`
- Prompt engineering decisions and revisions: `prompt-archive.md`, `ollama-plan.md`
- Ollama setup and reproducible run steps: `setup.md`
- Final gameplay loop, controls, dependencies, and fallback behavior: `readme.md`
- Iteration log, scope simplification, bug fixes, and design changes: `refinements-changes.md`
- Working implementation evidence in project scripts:
  - `Assets/Scripts/AI/OllamaClient.cs`
  - `Assets/Scripts/AI/PromptBuilder.cs`
  - `Assets/Scripts/Game/GameManager.cs`
  - `Assets/Scripts/Game/FallbackCaseProvider.cs`

## Video Planning
### Technical Demonstration Video
1. Show Ollama installed locally and confirm the server is running.
2. Show a terminal command proving the chosen model is available.
3. Open the Unity project and the main scene.
4. Explain that Unity sends a local HTTP POST request to `http://localhost:11434/api/generate`.
5. Briefly show `OllamaClient.cs` and the structured JSON schema in `PromptBuilder.cs`.
6. Press Play and generate a new case.
7. Show the generated case briefing with the crime, victim, and suspects.
8. Open one suspect and demonstrate the two generated follow-up questions.
9. Mention latency, fallback behavior, and why a single generation request was chosen for stability.

### Final Showcase Video
1. Show the main menu and title screen.
2. Start a new case and let Ollama generate the mystery.
3. Read the case briefing and introduce the suspects.
4. Open at least two suspects and ask their generated follow-up questions.
5. Show how the answers support deduction.
6. Accuse one suspect.
7. Show the result screen, clue, and explanation.
8. End with a short explanation of how the LLM improves replayability and narrative variety.
