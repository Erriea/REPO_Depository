namespace CaseFileLocalSuspect.Game
{
    public static class FallbackCaseProvider
    {
        public static CaseFile CreateCase(int caseNumber = 0)
        {
            switch (System.Math.Abs(caseNumber) % 3)
            {
                case 0:
                    return CreateMarloweLedgerTheft();
                case 1:
                    return CreateMuseumSabotage();
                default:
                    return CreateTheatreDisappearance();
            }
        }

        private static CaseFile CreateMarloweLedgerTheft()
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

        private static CaseFile CreateMuseumSabotage()
        {
            return new CaseFile
            {
                caseTitle = "The Silent Gallery Sabotage",
                crime = "Just before a high-profile unveiling, the climate control system in the Ashbourne Museum failed and nearly destroyed a priceless restoration project.",
                victim = "Dr. Owen Vale",
                victimPortraitId = "MascCharacter1",
                location = "Ashbourne Museum, restoration wing",
                guiltySuspect = "Mira Sloane",
                keyClue = "Mira claims she never entered the control room, but only someone inside could know the emergency override panel had been jammed with a brass palette knife.",
                explanation = "Mira Sloane sabotaged the restoration wing to ruin Dr. Vale's unveiling and force the museum to cancel the transfer of the painting she believed should remain under her care. Her answer reveals knowledge of the jammed override panel before investigators publicly mention it.",
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Jonah Reed",
                        portraitId = "MascCharacter2",
                        role = "Night technician",
                        connectionToCase = "He monitored the museum systems and responded first when the alarms triggered.",
                        motive = "Dr. Vale had filed complaints about Jonah's missed maintenance logs.",
                        alibi = "I was on the east staircase tracing a lighting fault when the temperature alarms started screaming through the wing.",
                        personality = "Nervous, practical, and eager to be believed",
                        openingStatement = "I know the systems, sure, but that also means I knew how bad this failure was the second it started. I ran toward the alarms, not away from them.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "What did you see when you reached the restoration wing?",
                                answer = "Dr. Vale was furious and the control room door was half open. I remember that because he kept shouting that someone had been inside before him."
                            },
                            new FollowUpQuestion
                            {
                                question = "Who had reason to target the unveiling?",
                                answer = "Plenty of people hated Vale's decisions, but Mira took this transfer personally. She argued with him about it all week."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Mira Sloane",
                        portraitId = "FemCharacter2",
                        role = "Lead conservator",
                        connectionToCase = "She had worked on the damaged painting for months and opposed moving it after the unveiling.",
                        motive = "She believed Dr. Vale was sacrificing the painting's safety for publicity.",
                        alibi = "I stayed in the prep studio cataloguing solvents. By the time I heard the alarm, Jonah and Vale were already ahead of me.",
                        personality = "Measured, proud, and quietly resentful",
                        openingStatement = "I cared more about that painting than anyone in the building. If someone damaged the environment around it, that someone was attacking my work too.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Why were you arguing with Dr. Vale before the unveiling?",
                                answer = "Because he was rushing a transfer I considered reckless. That was a professional disagreement, not sabotage."
                            },
                            new FollowUpQuestion
                            {
                                question = "How do you know what happened in the control room?",
                                answer = "Only from what I heard afterward. The override panel was jammed, wasn't it? That is what everyone is whispering now."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Clara Benn",
                        portraitId = "FemCharacter1",
                        role = "Museum donor liaison",
                        connectionToCase = "She organized the unveiling guests and was moving between the public hall and staff corridor all evening.",
                        motive = "A failed unveiling would embarrass her donors and threaten her role.",
                        alibi = "I was escorting two patrons back to the reception hall when the staff started closing doors around us.",
                        personality = "Polished, anxious, and image-conscious",
                        openingStatement = "Tonight was supposed to be flawless. I was trying to keep donors calm while your suspect turned a museum into a disaster scene.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Did you see anyone leaving the restoration wing?",
                                answer = "I saw Mira come from that direction earlier, before the alarms. She looked upset, but I thought it was just another argument with Vale."
                            },
                            new FollowUpQuestion
                            {
                                question = "What was the atmosphere like before the sabotage?",
                                answer = "Tense. Vale was playing showman, Mira was furious, and Jonah looked like he was trying not to be blamed for something before anything even happened."
                            }
                        }
                    }
                }
            };
        }

        private static CaseFile CreateTheatreDisappearance()
        {
            return new CaseFile
            {
                caseTitle = "The Velvet Curtain Disappearance",
                crime = "Minutes before opening night, the star performer vanished from the Grand Lyric Theatre and a ransom-style note was left in her dressing room.",
                victim = "Celeste Dane",
                victimPortraitId = "FemCharacter1",
                location = "Grand Lyric Theatre, backstage level",
                guiltySuspect = "Adrian Cross",
                keyClue = "Adrian insists the note used plain stationery from the lobby desk, but only someone backstage knew the theatre had switched to embossed dressing-room cards for opening night.",
                explanation = "Adrian Cross staged Celeste Dane's disappearance to force a last-minute promotion and sabotage her contract negotiations. His description of the note gives away that he handled the wrong stationery before the staff reported that detail.",
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Lena Morrell",
                        portraitId = "FemCharacter2",
                        role = "Stage manager",
                        connectionToCase = "She controlled backstage traffic and checked in every performer before curtain call.",
                        motive = "Celeste's unpredictability kept putting opening night at risk and reflected badly on Lena.",
                        alibi = "I was calling places from the cue desk when wardrobe reported Celeste missing from her dressing room.",
                        personality = "Sharp, overworked, and direct",
                        openingStatement = "I needed that show to start on time, not collapse. If Celeste disappeared, she either trusted the wrong person or someone backstage knew exactly when to move.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Who had the easiest access to Celeste before the show?",
                                answer = "Anyone with a backstage badge could get near her, but Adrian moved in and out of her hallway more than most. He always found a reason to linger."
                            },
                            new FollowUpQuestion
                            {
                                question = "What did you notice in the dressing room?",
                                answer = "The note looked staged to me. Too neat, too convenient, and left where it would absolutely be found before curtain."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Adrian Cross",
                        portraitId = "MascCharacter1",
                        role = "Understudy lead",
                        connectionToCase = "He would inherit the starring role if Celeste could not perform.",
                        motive = "Celeste's absence would put him in the spotlight he thought he deserved.",
                        alibi = "I was in rehearsal room B warming up alone. Ask anyone who heard me running my second act lines through the wall.",
                        personality = "Charming, ambitious, and slightly theatrical",
                        openingStatement = "Everyone loves blaming the understudy because it fits the story. Yes, her disappearance benefits me on paper, but that does not mean I engineered it.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "What do you know about the note left behind?",
                                answer = "Only that it looked rushed, like something scribbled on plain theatre stationery. Whoever did it wanted panic more than money."
                            },
                            new FollowUpQuestion
                            {
                                question = "How were things between you and Celeste this week?",
                                answer = "Competitive, naturally. She knew I could carry the show if I had to, and she hated being reminded of that."
                            }
                        }
                    },
                    new Suspect
                    {
                        name = "Noah Pike",
                        portraitId = "MascCharacter2",
                        role = "Lighting operator",
                        connectionToCase = "He worked the catwalk and saw who moved through the backstage corridor before places were called.",
                        motive = "Celeste had filed a complaint after Noah nearly dropped a spotlight cue during rehearsal.",
                        alibi = "I was in the lighting booth finalizing cues when house management called for a hold on the audience entry.",
                        personality = "Dry, observant, and hard to rattle",
                        openingStatement = "I watch people for a living, even when they forget that. Backstage was chaos, but not random chaos. Somebody timed this.",
                        followUpQuestions = new[]
                        {
                            new FollowUpQuestion
                            {
                                question = "Did you see anyone near Celeste's dressing room?",
                                answer = "Adrian passed that corridor twice in ten minutes. The second time, he was moving fast and suddenly very interested in not being noticed."
                            },
                            new FollowUpQuestion
                            {
                                question = "Did Celeste seem likely to run away on her own?",
                                answer = "No. She was difficult, not careless. If she vanished, someone either persuaded her out or cornered her at exactly the right moment."
                            }
                        }
                    }
                }
            };
        }
    }
}
