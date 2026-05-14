namespace CaseFileLocalSuspect.Game
{
    public static class PlaceholderCaseFactory
    {
        public static CaseFile CreatePreviewCase()
        {
            return new CaseFile
            {
                caseTitle = "The Missing Sapphire Ledger",
                crime = "A valuable ledger vanished from the back office of the Marlowe Club just before closing.",
                victim = "Evelyn Cross, the club owner",
                location = "The Marlowe Club, downtown",
                guiltySuspect = "Mina Vale",
                keyClue = "The guilty suspect gives a confident alibi that quietly conflicts with the office key log.",
                explanation = "This is placeholder text for the result screen and will be replaced by real case logic later.",
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Mina Vale",
                        role = "Bookkeeper",
                        motive = "She feared the ledger would expose falsified accounts.",
                        alibi = "She claims she never left the public lounge during the blackout.",
                        personality = "Composed and careful",
                        secret = "She knows the key cabinet was left unlocked."
                    },
                    new Suspect
                    {
                        name = "Jonah Pike",
                        role = "Bartender",
                        motive = "He owed money to a dangerous lender.",
                        alibi = "He says he was cleaning broken glass behind the bar.",
                        personality = "Defensive but talkative",
                        secret = "He heard someone moving near the office corridor."
                    },
                    new Suspect
                    {
                        name = "Clara Thorne",
                        role = "Singer",
                        motive = "She was angry over a cancelled contract.",
                        alibi = "She says she was in the dressing room changing costumes.",
                        personality = "Sharp and impatient",
                        secret = "She saw Mina carrying club records earlier that evening."
                    }
                }
            };
        }
    }
}
