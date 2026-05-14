# CaseFile: Local Suspect

## Project Title
- Working title: `CaseFile: Local Suspect`

## High Concept
- A beginner-friendly Unity 2D detective interrogation game where the player questions three AI-driven suspects and accuses the one they believe is guilty.

## Player Experience Goal
- Give the player the feeling of solving a short noir mystery through careful questioning rather than action gameplay.

## Core Gameplay Loop
1. Start a new case.
2. Read the case briefing.
3. Interrogate suspects with a limited number of typed questions.
4. Accuse one suspect.
5. Review the result and explanation.

## Setting and Tone
- Text-first noir detective presentation.
- Dark UI panels, suspect cards, and simple placeholder portraits.

## LLM Role
- A local Ollama model generates the hidden case file at the start of the round.
- The same local model answers interrogation questions in character for the selected suspect.

## Why Local Ollama Fits
- The assignment requires a local LLM workflow.
- Local inference supports reproducibility and keeps the project independent from cloud APIs.

## What Makes The Integration Meaningful
- The LLM is part of the core gameplay loop, not a cosmetic extra.
- The player’s custom questions directly affect the clues they uncover before making an accusation.

## Target Audience
- Players who enjoy short detective deduction games and text-based narrative systems.

## Scope Boundaries
- One main scene.
- UI-driven interaction only.
- No movement, combat, physics puzzles, or large content generation systems.
- Focus on a stable vertical slice that proves the Ollama integration works.

## Out Of Scope
- Multiplayer.
- Voice input.
- Branching cutscenes.
- Advanced animation systems.
- Large numbers of suspects or multiple crime scenes in one round.
