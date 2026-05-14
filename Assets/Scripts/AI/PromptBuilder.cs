using System.Text;
using CaseFileLocalSuspect.Game;

namespace CaseFileLocalSuspect.AI
{
    public static class PromptBuilder
    {
        public static string BuildCaseGenerationPrompt()
        {
            return
                "Create a fictional detective case for a Unity game. Return valid JSON only with this structure: " +
                "{\"case_title\":\"string\",\"crime\":\"string\",\"victim\":\"string\",\"location\":\"string\",\"suspects\":[{\"name\":\"string\",\"role\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"secret\":\"string\"},{\"name\":\"string\",\"role\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"secret\":\"string\"},{\"name\":\"string\",\"role\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"secret\":\"string\"}],\"guilty_suspect\":\"string\",\"key_clue\":\"string\",\"explanation\":\"string\"}. " +
                "Rules: exactly three suspects, no markdown, no code fences, the guilty suspect must exactly match one suspect name, keep the crime non-graphic, and make the case solvable through interrogation.";
        }

        public static string BuildInterrogationPrompt(CaseFile caseFile, Suspect suspect, string playerQuestion)
        {
            StringBuilder prompt = new StringBuilder();
            prompt.AppendLine("You are roleplaying as a suspect in a fictional detective game.");
            prompt.AppendLine("Answer in first person.");
            prompt.AppendLine("Stay consistent with the case file.");
            prompt.AppendLine("Use only the facts in the case file.");
            prompt.AppendLine("Do not invent new suspects, evidence, locations, or crimes.");
            prompt.AppendLine("Do not directly reveal the guilty suspect.");
            prompt.AppendLine("Do not say 'I am guilty.'");
            prompt.AppendLine("Keep the answer short: 2 to 5 sentences.");
            prompt.AppendLine();
            prompt.AppendLine($"Case title: {caseFile.caseTitle}");
            prompt.AppendLine($"Crime: {caseFile.crime}");
            prompt.AppendLine($"Victim: {caseFile.victim}");
            prompt.AppendLine($"Location: {caseFile.location}");
            prompt.AppendLine($"Selected suspect: {suspect.name}");
            prompt.AppendLine($"Role: {suspect.role}");
            prompt.AppendLine($"Motive: {suspect.motive}");
            prompt.AppendLine($"Alibi: {suspect.alibi}");
            prompt.AppendLine($"Personality: {suspect.personality}");
            prompt.AppendLine($"Secret: {suspect.secret}");
            prompt.AppendLine();
            prompt.AppendLine($"Player question: {playerQuestion}");
            return prompt.ToString();
        }
    }
}
