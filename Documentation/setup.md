# Setup Guide

## Purpose
- This document explains how to install the local model workflow and run the prototype from scratch.

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
- Recording tested hardware helps explain latency differences between machines in the assignment videos and report.

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

## How To Run The Prototype
1. Start Ollama or make sure the Ollama service is available.
2. Open the Unity project.
3. Open `MainScene`.
4. Press Play.
5. Click `New Case`.
6. Wait for Ollama to generate the case.

## Reproducible Test Sequence
1. Run `ollama --version`
2. Run `ollama pull llama3.2` if the model is missing
3. Run the API test command in this document
4. Open the Unity project
5. Press Play and generate a case
6. Confirm the game either loads a live generated case or falls back safely

## What The Game Now Does
- Generates one full detective case per round.
- Shows three suspects.
- Gives the player two generated follow-up questions per suspect.
- Lets the player accuse one suspect.
- Shows the hidden clue and explanation afterward.

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
- This is expected local inference behavior and should be mentioned in the technical demonstration video.

### Unity falls back instead of generating a live case
- Check that Ollama is running.
- Confirm the API test above works.
- Confirm the selected model is installed.

### Output keeps failing
- Try a smaller supported local model for testing.
- Confirm that no firewall rule is blocking `localhost:11434`.

## Notes For Demonstration
- Show Ollama running locally before opening Play mode.
- Show the terminal API test or the installed model list.
- Show that the game still works with fallback content if generation fails.
