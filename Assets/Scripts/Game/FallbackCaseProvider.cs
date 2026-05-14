namespace CaseFileLocalSuspect.Game
{
    public static class FallbackCaseProvider
    {
        public static CaseFile CreateCase()
        {
            return new CaseFile
            {
                caseTitle = "The Marlowe Ledger Theft",
                crime = "Late last night, the sealed financial ledger for the Marlowe Foundation vanished from the victim's office just before a public fraud hearing.",
                victim = "Victor Harrow",
                victimPortraitId = "MascCharacter1",
                location = "Harrow Estate, private study",
                guiltySuspect = "Nadia Price",
                keyClue = "Nadia insists the study door was already open, but only the victim and the culprit knew the door had been relocked after dinner.",
                explanation = "Nadia Price stole the ledger to stop Victor from revealing that she had been quietly rerouting foundation money. Her answers sound polished at first, but her story slips when she describes the study as already open. That detail conflicts with the butler's account and with the victim's habit of locking the study whenever he stepped away. The contradiction makes her the only suspect with both motive and hidden access.",
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Elena Voss",
                        portraitId = "FemCharacter1",
                        role = "Public relations advisor",
                        motive = "Victor planned to dismiss her after the hearing, which would ruin her reputation.",
                        alibi = "I was in the ballroom making a last phone call to the press office when the shouting started.",
                        personality = "Elegant, guarded, and quick to deflect",
                        secret = "She knew the hearing would be disastrous, but she never entered the study."
                    },
                    new Suspect
                    {
                        name = "Nadia Price",
                        portraitId = "FemCharacter2",
                        role = "Foundation accountant",
                        motive = "The missing ledger could expose that she altered grant records to hide stolen funds.",
                        alibi = "I only went looking for Victor after dinner because he missed the budget review. The study door was already open when I got there.",
                        personality = "Controlled, precise, and slightly defensive",
                        secret = "She memorized the victim's filing habits and knew where the ledger was kept."
                    },
                    new Suspect
                    {
                        name = "Marcus Flint",
                        portraitId = "MascCharacter2",
                        role = "Head of estate security",
                        motive = "Victor had threatened to fire him after a string of recent security failures.",
                        alibi = "I was outside checking the service gate after the storm knocked the lights for a minute.",
                        personality = "Blunt, tired, but direct",
                        secret = "He saw Nadia leave the hall near the study, but he is not certain what she was carrying."
                    }
                }
            };
        }
    }
}
