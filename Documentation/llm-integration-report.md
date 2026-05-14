# LLM Integration Report Draft

## Report Purpose
- This draft will become the 600 to 800 word assignment report covering the technical and design decisions behind the local LLM integration.

## Technical Decisions
- The project uses Unity with local HTTP requests to Ollama instead of a hosted model API.

## Integration Strategy
- Generate one complete hidden case file at the start of a round.
- Reuse the same stored case file for every suspect response.

## How Ollama Is Used
- Ollama generates structured case data.
- Ollama roleplays suspects during interrogation.

## Why Local Inference Is Appropriate
- Local inference matches the assignment requirement and makes the prototype self-contained.

## Gameplay Impact
- The LLM is central to the questioning and deduction loop.

## Performance Considerations
- Smaller local models may be used if latency becomes too high.

## Fallback Strategy
- A pre-written fallback case will be used if generation fails, times out, or returns invalid JSON.

## Limitations Of LLM Output
- Responses may still vary in tone, structure, or quality.
- Prompt constraints and fallback systems are needed for stability.

## Ethical Considerations
- The game uses fictional content only.
- AI usage should be documented clearly.
- Credits and tooling transparency should be included in the final submission.

## Workflow Evaluation
- To be expanded after implementation and testing.
