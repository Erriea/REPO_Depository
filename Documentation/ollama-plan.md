# Ollama Integration Plan

## Purpose of Ollama in This Project
- Ollama provides the local LLM used for case generation and suspect interrogation.

## Selected Model
- Planned starting model: `llama3.2`

## Alternative Models
- `mistral`
- `phi3`
- `gemma2:2b`

## Why The Model Was Chosen
- `llama3.2` is the recommended first model from the brief and is a strong starting point for a small local prototype.

## Where Inference Occurs
- Inference runs locally on the user’s computer through Ollama.

## When Inference Occurs
- Once when a new case is generated.
- Once per interrogation question while the player is questioning a suspect.

## Unity To Ollama Data Flow
1. Unity builds a prompt.
2. Unity sends an HTTP POST request to `http://localhost:11434/api/generate`.
3. Ollama returns a non-streamed text response.
4. Unity parses the result and updates the game state or UI.

## Prompt Structure Plan
- Case generation prompt requesting valid JSON only.
- Interrogation prompt containing the hidden case file, selected suspect profile, player question, and behavior rules.

## Reliability Rules
- Generate the full case once and store it in memory.
- Reuse the same hidden case file during all interrogations.
- Keep suspect responses short and in character.
- Prevent invented suspects, evidence, crimes, or locations.

## Risks And Mitigations
- Invalid JSON: clean the response and fall back to a hardcoded case if parsing fails.
- Slow responses: use smaller models if needed and show a loading message.
- Inconsistent answers: always resend the same hidden case file.
- Local setup issues: document installation, model pulling, and troubleshooting clearly.

## Latency Considerations
- The first prototype should favor reliability over depth.
- Non-streaming responses make Unity integration simpler to debug.

## Local Vs Cloud Considerations
- Local inference matches the assignment requirements and avoids paid API dependencies.
- Cloud models may be stronger, but they are outside the intended scope for this project.

## Reproducibility Notes
- The tested model name, Unity version, prompts, and fallback behavior must be documented as development continues.
