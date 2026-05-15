# CaseFile: Local Suspect

## Project Title
- Working title: `CaseFile: Local Suspect`

## High Concept
- A beginner-friendly Unity detective game where a local LLM generates a fresh crime, three suspects, and the interrogation content for each round.

## Player Experience Goal
- Give the player the feeling of solving a short mystery by comparing statements, spotting contradictions, and making a final accusation.

## Core Gameplay Loop
1. Start a new case.
2. Let Ollama generate the crime setup, suspects, and clue structure.
3. Read the case briefing.
4. Open each suspect and read their opening statement.
5. Ask two generated follow-up questions per suspect.
6. Compare the answers and accuse one suspect.
7. Review the result, key clue, and explanation.

## Setting And Tone
- Text-first detective presentation.
- Stylized suspect portraits and a case-board menu background.
- Light noir influence without graphic violence.

## LLM Role
- A local Ollama model generates the case title, crime hook, victim, location, suspect profiles, opening statements, follow-up questions, answers, culprit, clue, and explanation.
- The LLM output directly drives gameplay content rather than acting as background flavor text.

## Why Local Ollama Fits
- The brief specifically asks for a meaningful local LLM integration.
- Local inference keeps the prototype reproducible and avoids paid API dependencies.
- The project can be demonstrated offline once the model is installed.

## What Makes The Integration Meaningful
- The game round cannot exist until the LLM generates the case data.
- The suspects and clue logic change from case to case.
- The player’s accusation is based on generated statements and generated follow-up answers.

## Simplified Scope Choice
- Earlier versions aimed for free-text suspect interrogation.
- That approach was reduced to a safer design where Ollama generates the entire case package in one request.
- This keeps the LLM central to gameplay while making the prototype easier to debug and demonstrate.

## Target Audience
- Players who enjoy short deduction games and lightweight interactive fiction systems.

## Scope Boundaries
- One main scene.
- UI-driven interaction only.
- Three suspects per case.
- Two follow-up questions per suspect.
- One accusation per round.

## Out Of Scope
- Free-text conversation with unlimited follow-up turns.
- Voice input or TTS.
- Combat, movement, or puzzle navigation.
- Multiplayer features.
- Large branching cutscene systems.
