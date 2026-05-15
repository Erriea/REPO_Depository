# Ollama Integration Plan

## Purpose Of Ollama In This Project
- Ollama provides the local LLM used to generate the playable detective case for each round.

## Selected Model
- Primary tested model: `llama3.2`

## Alternative Models
- `mistral`
- `phi3`
- `gemma2:2b`

## Why The Model Was Chosen
- `llama3.2` is easy to install, widely recommended for local experimentation, and strong enough for a small structured narrative prototype.

## Where Inference Occurs
- Inference runs locally on the player or assessor machine through Ollama.

## When Inference Occurs
- Once when a new case is generated.
- The current final design avoids a second live call for every question.

## Inference Timing Notes
- First request timing is usually the slowest because the model may still be loading or waking up.
- Later case generations are typically faster once Ollama has the model ready in memory.
- The final build was intentionally designed around one request per round so that latency is easier to manage during demonstrations.
- Timing can vary depending on CPU, RAM, GPU support, and whether other programs are competing for resources.

## Final Integration Strategy
- Unity sends one structured generation request to Ollama.
- Ollama returns a JSON case package containing:
  - case title
  - crime summary
  - victim
  - location
  - 3 suspects
  - opening statement for each suspect
  - 2 follow-up questions and 2 answers for each suspect
  - guilty suspect
  - key clue
  - explanation
- Unity parses the JSON and uses it to drive the entire round.

## Unity To Ollama Data Flow
1. The player presses `New Case`.
2. `GameManager` requests a new case.
3. `PromptBuilder` creates the prompt and JSON schema.
4. `OllamaClient` sends a local HTTP POST request to `http://localhost:11434/api/generate`.
5. Ollama returns a non-streamed structured response.
6. Unity parses the response into `CaseFile`, `Suspect`, and `FollowUpQuestion` objects.
7. The UI displays the generated round.

## Prompt Structure
- A case generation prompt defines the tone, structure, and content rules.
- The request also includes a JSON schema to improve reliability.
- The prompt explicitly enforces:
  - exactly 3 suspects
  - exactly 2 follow-up questions per suspect
  - non-graphic crime content
  - a male victim
  - suspect order matching available portraits: female, female, male
  - a culprit name that matches one of the generated suspects exactly

## Reliability Rules
- Use one generation request per case instead of repeated live suspect calls.
- Force structured output with a schema.
- Keep the model temperature low for stability.
- Reject invalid responses and fall back to a prepared case.

## Risks And Mitigations
- Invalid JSON:
  - mitigated with JSON schema output and strict parsing.
- Slow first response:
  - mitigated with a loading state and setup notes that explain model warm-up time.
- Repeated or similar cases:
  - mitigated with case themes, random seeds, and a prompt instruction not to imitate the previous case.
- Local setup failure:
  - mitigated with a fallback mode and a beginner-friendly setup guide.

## Latency Considerations
- Local inference is slower than static content, especially on the first request.
- One request per case is faster and more stable than one request per interrogation turn.
- This was a deliberate scope decision to protect the final demo.
- In practical terms, the timing issue shaped the entire design: the game now spends its latency budget on one stronger generation step instead of many smaller risky steps.

## Local Vs Cloud Considerations
- Local inference offers lower cost, better transparency, and closer alignment with the assignment brief.
- Cloud models may produce stronger writing, but they add dependency, cost, and reproducibility concerns.
- For this project, control and local reproducibility were more important than maximum model quality.

## Reproducibility Notes
- Document the Unity version, tested model name, endpoint, prompts, schema usage, and fallback behavior.
- The final build should be demonstrated with Ollama visibly running on the local machine.
