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
- Open a terminal after installation.
- Check the installation with `ollama --version`.
- Pull the starter model with `ollama pull llama3.2`.
- Test the model with `ollama run llama3.2`.
- Ask for a short test sentence from a suspicious suspect.
- Exit the terminal chat when finished.

## Ollama Server Check
- Confirm the local server is available at `http://localhost:11434`.
- Unity will send requests to `http://localhost:11434/api/generate`.

## Unity Setup
- Open the project from `C:\Code\2026 Unity\REPO_Depository`.
- Open the main prototype scene.
- Press Play in the Unity Editor.
- If Ollama is not running, the game should use fallback behavior once implemented.

## Troubleshooting To Expand Later
- Ollama is not installed.
- The model is missing.
- The server is not running.
- Responses are slow.
- Unity cannot parse the response.

## Git Workflow Notes
- Commit frequently in small milestones.
- Suggested milestones will be recorded as the project develops.
