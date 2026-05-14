using System;

namespace CaseFileLocalSuspect.Game
{
    public static class FallbackDialogueGenerator
    {
        public static string GenerateResponse(CaseFile caseFile, Suspect suspect, string question, int questionsAskedSoFar)
        {
            string lowerQuestion = (question ?? string.Empty).Trim().ToLowerInvariant();
            bool isGuilty = string.Equals(caseFile.guiltySuspect, suspect.name, StringComparison.OrdinalIgnoreCase);

            if (ContainsAny(lowerQuestion, "who are you", "your name", "what do you do"))
            {
                return $"I'm {suspect.name}, the {suspect.role.ToLowerInvariant()}. {DescribeTone(suspect, isGuilty)}";
            }

            if (ContainsAny(lowerQuestion, "where were you", "alibi", "what were you doing", "during dinner", "when it happened"))
            {
                return $"I already told you where I was. {suspect.alibi} {AddPressureLine(isGuilty, questionsAskedSoFar)}";
            }

            if (ContainsAny(lowerQuestion, "motive", "why would you", "why did you", "money", "benefit"))
            {
                return $"People always assume the worst. {ToFirstPersonMotive(suspect.motive)} {AddPressureLine(isGuilty, questionsAskedSoFar)}";
            }

            if (ContainsAny(lowerQuestion, "victor", "victim", "harrow"))
            {
                if (isGuilty)
                {
                    return "Victor kept too many secrets and expected everyone else to clean up after him. That does not mean I wished him dead or wanted his papers.";
                }

                return $"Victor Harrow was difficult, but I knew how to work around him. {NonGuiltyVictimOpinion(suspect.name)}";
            }

            if (ContainsAny(lowerQuestion, "ledger", "record", "file", "documents", "study"))
            {
                if (isGuilty)
                {
                    return "You keep circling back to the ledger as if that proves something. I told you, the study door was already open when I reached it.";
                }

                return $"{suspect.secret} That's the only thing I can tell you about the study.";
            }

            if (ContainsAny(lowerQuestion, "door", "locked", "open", "key"))
            {
                if (isGuilty)
                {
                    return "I wasn't standing there inspecting the lock for you. The door was open, the room was empty, and that is all I saw.";
                }

                return "I never touched the study door myself. If you want the truth, someone who knew Victor's routine got there before the rest of us.";
            }

            if (ContainsAny(lowerQuestion, "secret", "hiding", "truth", "what aren't you telling me"))
            {
                if (isGuilty)
                {
                    return "I'm telling you enough to do your job, detective. You don't need every private detail of my work to solve this.";
                }

                return $"{suspect.secret} I kept quiet because I did not want to make trouble before I was certain.";
            }

            if (ContainsAny(lowerQuestion, "did you do it", "are you guilty", "did you steal"))
            {
                if (isGuilty)
                {
                    return "That's a lazy question, detective. If you're going to accuse me, you'd better do it with evidence.";
                }

                return "No. I may have had reasons to be angry, but I did not steal anything from that study.";
            }

            return GenericResponse(suspect, isGuilty);
        }

        private static string GenericResponse(Suspect suspect, bool isGuilty)
        {
            if (isGuilty)
            {
                return $"I've answered enough wandering questions already. Ask something specific, or stop trying to shake me with theater.";
            }

            return $"I'm trying to help, but that question is too vague. Ask me about my alibi, Victor, or what I noticed near the study.";
        }

        private static string DescribeTone(Suspect suspect, bool isGuilty)
        {
            if (isGuilty)
            {
                return "And before you ask, I have no patience for accusations built on nerves and gossip.";
            }

            return $"People keep calling me {suspect.personality.ToLowerInvariant()}, but that doesn't make me a criminal.";
        }

        private static string ToFirstPersonMotive(string motive)
        {
            return $"Yes, I had pressures of my own. {motive}";
        }

        private static string AddPressureLine(bool isGuilty, int questionsAskedSoFar)
        {
            if (!isGuilty)
            {
                return "If that sounds nervous, it's because being questioned for theft tends to make people tense.";
            }

            return questionsAskedSoFar >= 3
                ? "You're repeating yourself now, and I don't like where you're trying to push this."
                : "That should be enough for now unless you've found something real.";
        }

        private static string NonGuiltyVictimOpinion(string suspectName)
        {
            switch (suspectName)
            {
                case "Elena Voss":
                    return "He was ruthless with public image, but he still cared more about appearances than honesty.";
                case "Marcus Flint":
                    return "He trusted locks, schedules, and pressure more than he trusted people.";
                default:
                    return "He had enemies, but half of them came from his own secrets.";
            }
        }

        private static bool ContainsAny(string source, params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (source.Contains(values[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
