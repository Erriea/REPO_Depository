# Setup Guide

## Purpose
- This document explains how to install the local Ollama workflow and run the final refined version of `CaseFile: Local Suspect`.

## Prerequisites
- Unity Hub installed
- Unity Editor `6000.0.44f1`
- Ollama installed locally
- The required model pulled before testing

## Tested System Specs
- Operating system: `Microsoft Windows 11 Home Single Language` `10.0.26200`
- CPU: `13th Gen Intel(R) Core(TM) i7-13620H`
- CPU cores / logical processors: `10 / 16`
- RAM: `16 GB` installed memory
- GPU 1: `NVIDIA GeForce RTX 4050 Laptop GPU`
- GPU 2: `Intel(R) UHD Graphics`

## Why System Specs Matter
- Local LLM performance depends on the machine running Ollama.
- The first request may be slower on lower-spec systems or when the model is loading into memory for the first time.
- Recording tested hardware helps explain latency differences between machines in a presentation or demonstration.

## Project Path
- `C:\Code\2026 Unity\REPO_Depository`

## Unity Setup
1. Open Unity Hub.
2. Add or open the project folder.
3. Open `Assets/Scenes/MainScene.unity`.
4. Wait for scripts to compile.

## Ollama Installation
1. Download Ollama from the official Ollama website.
2. Install it for your operating system.
3. On Windows, open a new PowerShell window after installation.
4. Run:

```powershell
ollama --version
```

- If a version number appears, Ollama is installed correctly.

## Pull The Tested Model
```powershell
ollama pull llama3.2
```

## Quick Local Test
```powershell
ollama run llama3.2
```

- Ask for a short line such as:
  - `Write one short detective sentence.`
- Exit when done.

## Ollama API Check
- The Unity project sends requests to:
  - `http://localhost:11434/api/generate`

- PowerShell test:

```powershell
(Invoke-WebRequest -Method POST -Body '{"model":"llama3.2","prompt":"Say hello in one short sentence.","stream": false}' -Uri http://localhost:11434/api/generate).Content
```

- If the response includes a `response` field, the local API is working.

## How To Run The Final Build
1. Start Ollama or make sure the Ollama service is available.
2. Open the Unity project.
3. Open `Assets/Scenes/MainScene.unity`.
4. Press Play.
5. Choose either:
   - `New Case`
   - `Timed Case`
6. Wait while the game shows the loading overlay and requests case data from Ollama.
7. Play through the investigation.

## What The Final Game Now Does
- Generates one full detective case per round in a single structured Ollama request.
- Includes a fallback case system if generation fails.
- Shows:
  - a crime board summary
  - a crime details screen
  - a one-suspect-at-a-time suspect review screen
  - a shared-question interrogation screen
  - an arrest screen
  - an outcome screen
- Gives the player `4` shared interrogation questions total.
- Includes a `Timed Case` mode with a `1 minute` countdown.
- Forces the player into the arrest decision when timed mode expires.

## Reproducible Test Sequence
1. Run `ollama --version`
2. Run `ollama pull llama3.2` if the model is missing
3. Run the API test command in this document
4. Open the Unity project
5. Press Play
6. Test `New Case`
7. Test `Timed Case`
8. Confirm the game either loads a live case or falls back safely

## Controls
- Mouse for all interaction:
  - main menu selection
  - panel navigation
  - suspect selection
  - question selection
  - arrest confirmation

## Troubleshooting

### `ollama` command not found
- Close and reopen PowerShell after installation.

### Model missing
- Run:

```powershell
ollama pull llama3.2
```

### Slow first response
- The model may still be warming up in memory.
- Wait for the first generation request to complete.
- This is expected local inference behavior.

### Unity falls back instead of generating a live case
- Check that Ollama is running.
- Confirm the API test above works.
- Confirm the selected model is installed.

### Output keeps failing
- Confirm that no firewall rule is blocking `localhost:11434`.
- Try restarting Ollama.
- Use the fallback path during demonstration if necessary, because the build is designed to remain playable.

### Timed mode immediately forces arrest
- Confirm that the timer has not already expired during testing.
- Restart the run from the main menu if necessary.

## Notes For Demonstration
- Show Ollama running locally before entering Play mode.
- Show both `New Case` and `Timed Case`.
- Show the loading overlay while a case is being generated.
- Show that the game still works with fallback content if live generation fails.
- Show that interrogation is limited to `4` shared questions and that question choice matters.
