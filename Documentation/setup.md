# Setup Guide

## Purpose
- This document explains how to set up and run the project from scratch as a beginner.

## Prerequisites
- Unity Hub installed.
- The correct Unity Editor version installed.
- Ollama installed locally.
- The required local model pulled before testing the game.

## Unity Version Used
- Current project version: `6000.0.44f1`

## Project Setup Overview
1. Open the project in Unity Hub.
2. Open the main scene used for the prototype.
3. Start Ollama before pressing Play.
4. Generate a case and test the UI flow.

## Ollama Setup
- Go to the official Ollama website.
- Download and install Ollama for your operating system.
- On Windows, open a new PowerShell window after installation.
- Run `ollama` to confirm the command exists.
- Pull and test the starter model with `ollama run llama3.2`.
- Ask for a short test sentence from a suspicious suspect.
- Exit the terminal chat when finished.

## Ollama Server Check
- Confirm the local server is available at `http://localhost:11434`.
- Unity will send requests to `http://localhost:11434/api/generate`.
- PowerShell API test:

```powershell
(Invoke-WebRequest -Method POST -Body '{"model":"llama3.2","prompt":"Say hello in one short sentence.","stream": false}' -Uri http://localhost:11434/api/generate).Content
```

- If the command returns JSON with a `response` field, the local API is working.

## Unity Setup
- Open the project from `C:\Code\2026 Unity\REPO_Depository`.
- Open the main prototype scene.
- Press Play in the Unity Editor.
- If Ollama is not running, the game should use fallback behavior once implemented.

## Troubleshooting To Expand Later
- `ollama` command not found: close and reopen PowerShell after installation.
- Model missing: run `ollama run llama3.2`.
- Server not running: relaunch Ollama from the Start menu.
- Slow responses: the first request may be loading the model. If needed, try `llama3.2:1b`.
- Unity cannot parse the response: check that requests use raw JSON and `stream: false`.

## Git Workflow Notes
- Commit frequently in small milestones.
- Suggested milestones will be recorded as the project develops.
