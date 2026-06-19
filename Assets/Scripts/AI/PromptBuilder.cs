using System.Text;

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

        public static string BuildCaseGenerationPrompt(int caseNumber, int varietySeed, string previousCaseTitle, int previousGuiltySuspectIndex, int requiredGuiltySuspectIndex)
        {
            string chosenTheme = CaseThemes[System.Math.Abs(varietySeed) % CaseThemes.Length];
            string repetitionGuard = string.IsNullOrWhiteSpace(previousCaseTitle)
                ? string.Empty
                : $"Do not reuse or closely imitate the previous case titled \"{previousCaseTitle}\". ";
            string culpritGuard = previousGuiltySuspectIndex >= 0
                ? $"Do not make suspect {previousGuiltySuspectIndex + 1} the guilty suspect again this time. "
                : string.Empty;
            string requiredCulpritInstruction = requiredGuiltySuspectIndex >= 0
                ? $"For variety, prefer suspect {requiredGuiltySuspectIndex + 1} as the guilty suspect this time if it fits naturally. "
                : string.Empty;

            return
                $"Create a fictional detective case for a Unity game. This is case run {caseNumber} with variation seed {varietySeed}. " +
                $"Use this case theme: {chosenTheme}. " +
                repetitionGuard +
                culpritGuard +
                requiredCulpritInstruction +
                "Make this case feel like a strong narrative hook for a player. Vary the victim role, location, type of incident, motives, and clue structure. " +
                "The victim must be male. The suspects must be ordered like this: suspect 1 female, suspect 2 female, suspect 3 male. Choose names, roles, and dialogue that clearly fit those genders. " +
                "Return valid JSON only with this structure: " +
                "{\"case_title\":\"string\",\"board_summary\":\"string\",\"victim\":\"string\",\"victim_description\":\"string\",\"location\":\"string\",\"crime\":\"string\",\"on_site_clues\":[\"string\",\"string\",\"string\"],\"interrogation_questions\":[\"string\",\"string\",\"string\",\"string\"],\"suspects\":[{\"name\":\"string\",\"role\":\"string\",\"description\":\"string\",\"appearance\":\"string\",\"connection_to_case\":\"string\",\"last_seen_victim\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"interrogation_answers\":[\"string\",\"string\",\"string\",\"string\"]},{\"name\":\"string\",\"role\":\"string\",\"description\":\"string\",\"appearance\":\"string\",\"connection_to_case\":\"string\",\"last_seen_victim\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"interrogation_answers\":[\"string\",\"string\",\"string\",\"string\"]},{\"name\":\"string\",\"role\":\"string\",\"description\":\"string\",\"appearance\":\"string\",\"connection_to_case\":\"string\",\"last_seen_victim\":\"string\",\"motive\":\"string\",\"alibi\":\"string\",\"personality\":\"string\",\"opening_statement\":\"string\",\"interrogation_answers\":[\"string\",\"string\",\"string\",\"string\"]}],\"guilty_suspect\":\"string\",\"guilt_clues\":[\"string\",\"string\"],\"explanation\":\"string\"}. " +
                "Rules: exactly three suspects, exactly four shared interrogation questions, exactly four interrogation answers per suspect, exactly three on-site clues, exactly two guilt clues, no markdown, no code fences, and the guilty suspect must exactly match one suspect name. " +
                "Keep the crime non-graphic and solvable from the on-site clues plus the interrogation answers. " +
                "Each opening_statement should sound like the suspect is speaking directly to the detective in first person. " +
                "Each interrogation answer should be 1 to 3 sentences, in character, and consistent with the hidden truth of the case. " +
                "The four interrogation questions must be broad enough that any suspect can answer them, but specific enough that asking the best question to the right suspect reveals useful contradictions or hidden knowledge. " +
                "At least two of the four interrogation questions must directly probe the crime scene clues, the timeline of the crime, or a suspicious detail already introduced elsewhere in the case. " +
                "Avoid generic filler questions. Every question should help test the evidence, the suspect's story, or the victim's conflict with that suspect. " +
                "The on-site clues and guilt clues must be concrete and helpful rather than vague. " +
                "Do not rely on the same resolution pattern every time. Avoid repeating the generic combination of torn fabric or matching material plus a weak alibi unless the case has genuinely distinctive supporting evidence beyond that. " +
                "Prefer varied clue types such as hidden knowledge, timeline contradictions, access-only details, misplaced tools, tampered documents, unique witness descriptions, overheard phrases, missing personal items, or scene-specific residue. " +
                "Very important: the guilt_clues and explanation may only reference evidence that the player has already seen in one of these places: the crime description, the on_site_clues, the suspect descriptions, or the interrogation answers. " +
                "Do not invent new evidence in the final explanation. Do not mention clothing details, scents, objects, witness facts, or physical traces unless those exact details were already surfaced earlier in the generated case data. " +
                "Make the location, victim_description, crime summary, suspect descriptions, alibis, motives, and last_seen_victim details narrative and atmospheric so the case feels like a real scene rather than a checklist. " +
                "The crime field must read like a short detective-novel scene and include how the victim died, where the body was found, and why the death matters to the people around him. " +
                "The victim_description should mention what kind of man he was, any relevant family or professional ties, and why someone might have wanted him dead. " +
                "Each suspect description should mention personality, relationship to the victim, and at least one physical detail the detective would notice on first meeting. Avoid meaningless filler like height unless it directly matters to the evidence. " +
                "If a clue depends on clothing, jewelry, a pin, fabric, scent, handwriting, or another identifying feature, that exact feature must appear earlier in the matching suspect's description, appearance, or interrogation answers. " +
                "Each on_site_clue should not only describe the evidence but also hint why it matters or what it suggests. " +
                "The guilt_clues must point to one clear culprit only. Do not split guilt across two suspects, and do not make the explanation rely on two different people being equally guilty. " +
                "At least one guilt clue must describe a specific contradiction, piece of insider knowledge, or concrete object trace that singles out the culprit instead of merely saying the culprit had motive or a weak alibi. " +
                "The culprit can be suspect 1, 2, or 3. Follow any variety preference given above when it still produces the strongest case, and do not default to suspect 1. " +
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
                    "\"board_summary\":{\"type\":\"string\"}," +
                    "\"victim\":{\"type\":\"string\"}," +
                    "\"victim_description\":{\"type\":\"string\"}," +
                    "\"location\":{\"type\":\"string\"}," +
                    "\"crime\":{\"type\":\"string\"}," +
                    "\"on_site_clues\":{" +
                        "\"type\":\"array\"," +
                        "\"minItems\":3," +
                        "\"maxItems\":3," +
                        "\"items\":{\"type\":\"string\"}" +
                    "}," +
                    "\"interrogation_questions\":{" +
                        "\"type\":\"array\"," +
                        "\"minItems\":4," +
                        "\"maxItems\":4," +
                        "\"items\":{\"type\":\"string\"}" +
                    "}," +
                    "\"suspects\":{" +
                        "\"type\":\"array\"," +
                        "\"minItems\":3," +
                        "\"maxItems\":3," +
                        "\"items\":{" +
                            "\"type\":\"object\"," +
                            "\"properties\":{" +
                                "\"name\":{\"type\":\"string\"}," +
                                "\"role\":{\"type\":\"string\"}," +
                                "\"description\":{\"type\":\"string\"}," +
                                "\"appearance\":{\"type\":\"string\"}," +
                                "\"connection_to_case\":{\"type\":\"string\"}," +
                                "\"last_seen_victim\":{\"type\":\"string\"}," +
                                "\"motive\":{\"type\":\"string\"}," +
                                "\"alibi\":{\"type\":\"string\"}," +
                                "\"personality\":{\"type\":\"string\"}," +
                                "\"opening_statement\":{\"type\":\"string\"}," +
                                "\"interrogation_answers\":{" +
                                    "\"type\":\"array\"," +
                                    "\"minItems\":4," +
                                    "\"maxItems\":4," +
                                    "\"items\":{\"type\":\"string\"}" +
                                "}" +
                            "}," +
                            "\"required\":[\"name\",\"role\",\"description\",\"appearance\",\"connection_to_case\",\"last_seen_victim\",\"motive\",\"alibi\",\"personality\",\"opening_statement\",\"interrogation_answers\"]" +
                        "}" +
                    "}," +
                    "\"guilty_suspect\":{\"type\":\"string\"}," +
                    "\"guilt_clues\":{" +
                        "\"type\":\"array\"," +
                        "\"minItems\":2," +
                        "\"maxItems\":2," +
                        "\"items\":{\"type\":\"string\"}" +
                    "}," +
                    "\"explanation\":{\"type\":\"string\"}" +
                "}," +
                "\"required\":[\"case_title\",\"board_summary\",\"victim\",\"victim_description\",\"location\",\"crime\",\"on_site_clues\",\"interrogation_questions\",\"suspects\",\"guilty_suspect\",\"guilt_clues\",\"explanation\"]" +
                "}";
        }
    }
}
