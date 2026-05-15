# Prompt Archive

## Archive Purpose
- This document records the prompt engineering work used in `CaseFile: Local Suspect`.
- It focuses on what was tested, what failed, what succeeded, and why the prompt strategy changed over time.
- It is written as assignment evidence, not just as internal notes.

## Final Prompt Strategy
- The final prototype uses one Ollama request at the start of each case.
- Unity sends:
  - one natural-language case generation prompt
  - one JSON schema
  - `stream: false`
  - `temperature: 0`
- The generated case package includes:
  - case title
  - crime
  - victim
  - location
  - three suspects
  - one opening statement per suspect
  - two follow-up questions and answers per suspect
  - guilty suspect
  - key clue
  - explanation

## Why The Prompt Strategy Changed
- The original plan was a live interrogation system where the player typed any question and Ollama answered in character every time.
- That idea was strong creatively, but weak technically for a small student prototype.
- Repeated live calls created too many risks:
  - higher latency
  - inconsistent suspect behavior
  - invented facts
  - more HTTP failure points
  - more parsing and state-management complexity
- The final design moved to one structured request per case because it was more stable, easier to demo, and easier to document.

## Final Prompts Used

### 1. Final Case Generation Prompt
Source: [PromptBuilder.cs](/C:/Code/2026%20Unity/REPO_Depository/Assets/Scripts/AI/PromptBuilder.cs)

```text
Create a fictional detective case for a Unity game. This is case run {caseNumber} with variation seed {varietySeed}. Use this case theme: {chosenTheme}. Do not reuse or closely imitate the previous case titled "{previousCaseTitle}" when provided. Make this case feel like a strong narrative hook for a player. Vary the victim role, location, type of incident, motives, and clue structure. The victim must be male. The suspects must be ordered like this: suspect 1 female, suspect 2 female, suspect 3 male. Choose names, roles, and dialogue that clearly fit those genders. Return valid JSON only with this structure: {"case_title":"string","crime":"string","victim":"string","location":"string","suspects":[{"name":"string","role":"string","connection_to_case":"string","motive":"string","alibi":"string","personality":"string","opening_statement":"string","questions":[{"question":"string","answer":"string"},{"question":"string","answer":"string"}]},{"name":"string","role":"string","connection_to_case":"string","motive":"string","alibi":"string","personality":"string","opening_statement":"string","questions":[{"question":"string","answer":"string"},{"question":"string","answer":"string"}]},{"name":"string","role":"string","connection_to_case":"string","motive":"string","alibi":"string","personality":"string","opening_statement":"string","questions":[{"question":"string","answer":"string"},{"question":"string","answer":"string"}]}],"guilty_suspect":"string","key_clue":"string","explanation":"string"}. Rules: exactly three suspects, exactly two follow-up questions per suspect, no markdown, no code fences, and the guilty suspect must exactly match one suspect name. Keep the crime non-graphic and solvable from the suspect statements and follow-up answers. Each opening_statement should sound like the suspect is speaking directly to the detective in first person. Each answer should be 1 to 3 sentences, in character, and consistent with the hidden truth of the case. Allowed case types include theft, disappearance, sabotage, blackmail, poisoning, fraud, or murder. Use short, believable names and roles. Keep the explanation concise and specific to the clue.
```

### 2. Final Structured Output Schema
Source: [PromptBuilder.cs](/C:/Code/2026%20Unity/REPO_Depository/Assets/Scripts/AI/PromptBuilder.cs)

Summary:
- object root
- required top-level fields
- exactly 3 suspects
- exactly 2 follow-up questions per suspect
- required suspect fields including:
  - `connection_to_case`
  - `opening_statement`
  - `questions`

Reason for using a schema:
- plain “return JSON only” instructions were not reliable enough on their own
- schema guidance improved structural consistency and Unity parsing reliability

### 3. Final Ollama Request Settings
Source: [OllamaClient.cs](/C:/Code/2026%20Unity/REPO_Depository/Assets/Scripts/AI/OllamaClient.cs)

The final request uses:

```json
{
  "model": "llama3.2",
  "prompt": "PROMPT_TEXT_HERE",
  "stream": false,
  "format": "json or schema",
  "options": {
    "temperature": 0
  }
}
```

Reasoning:
- `stream: false` keeps the Unity response handling simple
- `temperature: 0` improves determinism and reduces format drift
- schema-based `format` reduces structural errors

## Tested Prompt Versions

### Tested Version A: Minimal JSON-Only Prompt
Example:

```text
Create a detective case and return JSON only.
```

Result:
- too weak
- missing fields
- wrong suspect count
- malformed JSON
- culprit name mismatched the suspect list

Verdict:
- failed

### Tested Version B: Expanded Plain-Language JSON Prompt
Example:

```text
Create a fictional detective case for a Unity game. Return valid JSON only with a title, crime, victim, location, suspects, guilty suspect, key clue, and explanation.
```

Result:
- better than the minimal prompt
- still unreliable on weaker local runs
- sometimes omitted nested details
- sometimes returned the right fields with the wrong internal shape

Verdict:
- partially successful, but not stable enough

### Tested Version C: Live Interrogation Roleplay Prompt
Example direction:

```text
You are roleplaying as the selected suspect in a fictional detective game. Answer the detective in first person. Stay consistent with the case file. Do not invent new evidence or reveal guilt directly.
```

Result:
- creatively interesting
- too risky for a time-limited assignment build
- repeated HTTP calls made the prototype slower and harder to trust
- answers could drift or contradict the stored case

Verdict:
- not adopted in the final build

### Tested Version D: Full Structured Case Package Prompt
This is the final adopted version.

Result:
- best overall reliability
- easier to parse
- easier to present in the UI
- easier to explain in documentation and videos

Verdict:
- successful and adopted

## Successful Prompt Examples

### Successful Pattern 1
- Strong narrative hook
- Exactly three suspects
- Correct portrait-compatible suspect order: female, female, male
- Male victim
- Clear key clue
- Short explanations that fit the accusation screen

Why it worked:
- clear constraints
- fixed suspect count
- fixed answer count
- schema-backed output

### Successful Pattern 2
- Variation prompts using:
  - `case run {caseNumber}`
  - `variation seed {varietySeed}`
  - rotating case themes
  - previous-title repetition guard

Why it worked:
- helped reduce repeated cases
- created stronger assignment evidence for iteration and replayability

## Failed Output Examples

### Failed Example 1: Invalid JSON
Observed problem:
- missing commas
- extra prose before the JSON
- markdown fences around output

Why it failed:
- prompt wording alone was not enough

Fix:
- schema-backed output
- stronger “valid JSON only” instruction
- `temperature: 0`

### Failed Example 2: Wrong Suspect Structure
Observed problem:
- fewer than 3 suspects
- more than 3 suspects
- suspects missing nested question data

Why it failed:
- under-specified output structure

Fix:
- hard `minItems` and `maxItems`
- exact required fields in the schema

### Failed Example 3: Portrait Mismatch
Observed problem:
- generated suspect names or roles implied the wrong gender for the available art
- victim sometimes did not match the intended portrait setup

Why it failed:
- the prompt did not originally account for fixed project art

Fix:
- added:
  - male victim rule
  - female, female, male suspect order
  - explicit instruction that names and roles should match those genders

### Failed Example 4: Repeated Cases
Observed problem:
- multiple cases felt too similar in role, tone, or clue structure

Why it failed:
- the prompt allowed the model to fall back on familiar patterns

Fix:
- added:
  - random variation seed
  - rotating theme list
  - instruction not to closely imitate the previous case title

## Iteration Notes And Reasoning

### Iteration 1
- Goal: prove Ollama could generate a detective case
- Prompt style: very simple JSON request
- Lesson: the model needed much stronger structure

### Iteration 2
- Goal: improve output completeness
- Prompt style: longer plain-language instructions
- Lesson: better, but still not reliable enough for Unity parsing

### Iteration 3
- Goal: use Ollama for free-text suspect interrogation
- Prompt style: roleplay prompts per question
- Lesson: too unstable and too slow for a student prototype under deadline

### Iteration 4
- Goal: redesign around reliability
- Prompt style: one complete structured case payload per round
- Lesson: this was the most practical design for the assignment

### Iteration 5
- Goal: align generated content with available character portraits
- Prompt style: enforce victim and suspect gender order
- Lesson: prompt design had to consider UI assets, not only narrative quality

### Iteration 6
- Goal: reduce repetition and improve replay value
- Prompt style: case themes, run count, variety seed, previous-case guard
- Lesson: replay variety is partly a prompt-engineering problem, not only a content problem

## Reasoning Behind The Final Prompt
- It balances creativity with structure.
- It supports the assignment requirement for meaningful LLM integration.
- It is easier to test and reproduce than fully live AI dialogue.
- It works better with Unity’s parsing and UI constraints.
- It fits the user’s beginner-friendly, deadline-sensitive workflow.

## Final Prompt Engineering Conclusions
- The best result came from combining:
  - strong natural-language instructions
  - exact output structure
  - schema-based validation guidance
  - low temperature
  - content constraints tied to actual game assets
- The most important lesson from this project is that prompt quality is not just about “better writing.”
- Good prompts also have to match:
  - engine limitations
  - UI design
  - available art
  - demo reliability
  - assignment scope
