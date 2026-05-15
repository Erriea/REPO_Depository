using System.Text;
using CaseFileLocalSuspect.Game;

namespace CaseFileLocalSuspect.AI
{
    public static class PromptBuilder
    {
        private static readonly string[] CaseThemes =
        {
            "a disappearance at a theatre",
            "a sabotage case inside a research lab",
            "a blackmail case at a charity gala",
            "a suspicious death inside a coastal hotel",
            "a missing evidence case at a museum",
            "a poisoning attempt at a manor dinner",
            "a financial fraud case in a family business",
            "a stolen prototype case at a technology expo",
            "an art theft at a private collection",
            "a murder in a railway station office"
        };

        public static string BuildCaseGenerationPrompt(int caseNumber, int varietySeed, string previousCaseTitle)
        {
            string chosenTheme = CaseThemes[System.Math.Abs(varietySeed) % CaseThemes.Length];
            string repetitionGuard = string.IsNullOrWhiteSpace(previousCaseTitle)
                ? string.Empty
                : $"Do not reuse or closely imitate the previous case titled \"{previousCaseTitle}\". ";

            return
                $"Create a fictional detective case for a Unity game. This is case run {caseNumber} with variation seed {varietySeed}. " +
                $"Use this case theme: {chosenTheme}. " +
                repetitionGuard +
                "Make this case feel like a strong narrative hook for a player. Vary the victim role, location, type of incident, motives, and clue structure. " +
                "The victim must be male. The suspects must be ordered like this: suspect 1 female, suspect 2 female, suspect 3 male. Choose names, roles, and dialogue that clearly fit those genders. " +
                "Return valid JSON only with this structure: " +
                "{\"case_title\":\"string\",\"crime\":\"string\",\"victim\":\"string\",\"location\":\"string\",\"suspects\":[{\"name\":\"string\",\"role\":\"string\",\"connection_to_case\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"questions\":[{\"question\":\"string\",\"answer\":\"string\"},{\"question\":\"string\",\"answer\":\"string\"}]},{\"name\":\"string\",\"role\":\"string\",\"connection_to_case\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"questions\":[{\"question\":\"string\",\"answer\":\"string\"},{\"question\":\"string\",\"answer\":\"string\"}]},{\"name\":\"string\",\"role\":\"string\",\"connection_to_case\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"questions\":[{\"question\":\"string\",\"answer\":\"string\"},{\"question\":\"string\",\"answer\":\"string\"}]}],\"guilty_suspect\":\"string\",\"key_clue\":\"string\",\"explanation\":\"string\"}. " +
                "Rules: exactly three suspects, exactly two follow-up questions per suspect, no markdown, no code fences, and the guilty suspect must exactly match one suspect name. " +
                "Keep the crime non-graphic and solvable from the suspect statements and follow-up answers. " +
                "Each opening_statement should sound like the suspect is speaking directly to the detective in first person. " +
                "Each answer should be 1 to 3 sentences, in character, and consistent with the hidden truth of the case. " +
                "Allowed case types include theft, disappearance, sabotage, blackmail, poisoning, fraud, or murder. " +
                "Use short, believable names and roles. Keep the explanation concise and specific to the clue.";
        }

        public static string BuildCaseGenerationSchema()
        {
            return
                "{" +
                "\"type\":\"object\"," +
                "\"properties\":{" +
                    "\"case_title\":{\"type\":\"string\"}," +
                    "\"crime\":{\"type\":\"string\"}," +
                    "\"victim\":{\"type\":\"string\"}," +
                    "\"location\":{\"type\":\"string\"}," +
                    "\"suspects\":{" +
                        "\"type\":\"array\"," +
                        "\"minItems\":3," +
                        "\"maxItems\":3," +
                        "\"items\":{" +
                            "\"type\":\"object\"," +
                            "\"properties\":{" +
                                "\"name\":{\"type\":\"string\"}," +
                                "\"role\":{\"type\":\"string\"}," +
                                "\"connection_to_case\":{\"type\":\"string\"}," +
                                "\"motive\":{\"type\":\"string\"}," +
                                "\"alibi\":{\"type\":\"string\"}," +
                                "\"personality\":{\"type\":\"string\"}," +
                                "\"opening_statement\":{\"type\":\"string\"}," +
                                "\"questions\":{" +
                                    "\"type\":\"array\"," +
                                    "\"minItems\":2," +
                                    "\"maxItems\":2," +
                                    "\"items\":{" +
                                        "\"type\":\"object\"," +
                                        "\"properties\":{" +
                                            "\"question\":{\"type\":\"string\"}," +
                                            "\"answer\":{\"type\":\"string\"}" +
                                        "}," +
                                        "\"required\":[\"question\",\"answer\"]" +
                                    "}" +
                                "}" +
                            "}," +
                            "\"required\":[\"name\",\"role\",\"connection_to_case\",\"motive\",\"alibi\",\"personality\",\"opening_statement\",\"questions\"]" +
                        "}" +
                    "}," +
                    "\"guilty_suspect\":{\"type\":\"string\"}," +
                    "\"key_clue\":{\"type\":\"string\"}," +
                    "\"explanation\":{\"type\":\"string\"}" +
                "}," +
                "\"required\":[\"case_title\",\"crime\",\"victim\",\"location\",\"suspects\",\"guilty_suspect\",\"key_clue\",\"explanation\"]" +
                "}";
        }
    }
}
