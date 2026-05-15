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
                        connectionToCase = "She managed the foundation's media strategy and was in the house preparing for the hearing.",
                        motive = "Victor planned to dismiss her after the hearing, which would ruin her reputation.",
                        alibi = "I was in the ballroom making a last phone call to the press office when the shouting started.",
                        personality = "Elegant, guarded, and quick to deflect",
                        openingStatement = "Victor liked to keep everyone tense before an announcement. I was preparing the press office notes when the house suddenly erupted, and by then everyone was already accusing everyone else.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "What was your relationship with Victor before the hearing?",
                                answer = "Professional, mostly. He could be ruthless, and yes, he planned to push me out, but that does not mean I walked into his study and stole anything."
                            },
                            new FollowUpQuestion
                            {
                                question = "Did you see anyone near the study before the ledger disappeared?",
                                answer = "Not clearly. I remember movement in the corridor, but I was focused on my call and I only caught the tail end of someone heading away from that part of the house."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Nadia Price",
                        portraitId = "FemCharacter2",
                        role = "Foundation accountant",
                        connectionToCase = "She controlled the financial records and was meant to brief Victor before the public hearing.",
                        motive = "The missing ledger could expose that she altered grant records to hide stolen funds.",
                        alibi = "I only went looking for Victor after dinner because he missed the budget review. The study door was already open when I got there.",
                        personality = "Controlled, precise, and slightly defensive",
                        openingStatement = "I was trying to prevent a public disaster, not cause one. Victor missed our budget review, so I went to find him, and the study looked disturbed before I touched anything.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Why were you so concerned about the ledger that night?",
                                answer = "Because the hearing depended on it. Without the ledger, Victor would be cornered by questions he could not answer, and the entire foundation would look compromised."
                            },
                            new FollowUpQuestion
                            {
                                question = "How did you know the study door was open?",
                                answer = "Because I saw it with my own eyes when I arrived. I reached the corridor, noticed the door ajar, and stepped in to see whether Victor had left in a hurry."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Marcus Flint",
                        portraitId = "MascCharacter2",
                        role = "Head of estate security",
                        connectionToCase = "He was responsible for the estate corridors, locks, and movement logs that night.",
                        motive = "Victor had threatened to fire him after a string of recent security failures.",
                        alibi = "I was outside checking the service gate after the storm knocked the lights for a minute.",
                        personality = "Blunt, tired, but direct",
                        openingStatement = "I know how this looks. Security failures land on me first, but I was dealing with the service gate when the lights dipped and that is exactly why I was away from the study.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Did the power issue create an opportunity for the thief?",
                                answer = "It created confusion, yes, but not a total blackout. Anyone who already knew the house and the timing could use that minute of distraction."
                            },
                            new FollowUpQuestion
                            {
                                question = "Did you notice anything unusual in the corridor afterward?",
                                answer = "I saw Nadia near that wing after the disturbance settled. I cannot swear I saw the ledger in her hands, but she was the one person leaving that corridor in a hurry."
                            }
                        }
                    }
                }
            };
        }
    }
}
