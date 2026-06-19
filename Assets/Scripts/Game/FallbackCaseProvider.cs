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
                    return CreateTheatreMurder();
            }
        }

        private static CaseFile CreateMarloweLedgerTheft()
        {
            return new CaseFile
            {
                caseTitle = "The Marlowe Ledger Theft",
                boardSummary = "The financial ledger that could expose fraud inside the Marlowe Foundation vanished just before a public hearing. Three insiders had motive, access, and something to hide.",
                victimDescription = "Victor Harrow, the foundation's severe and deeply controlling director, had spent years making enemies with his need to dominate every room he entered. He had no patience for weakness, little affection for the people who worked under him, and more than enough secrets in his financial records to give someone a reason to silence him.",
                crime = "Late last night, Victor Harrow withdrew to his private study with the sealed Marlowe Foundation ledger, intending to face a public fraud hearing in the morning. Before he could speak, the ledger vanished and the people closest to the foundation began scrambling to protect themselves.",
                victim = "Victor Harrow",
                victimPortraitId = "MascCharacter1",
                location = "Harrow Estate, private study",
                onSiteClues = new[]
                {
                    "The study door had been relocked after dinner, which means whoever entered later either had access or was let in.",
                    "A smudged accounting glove print was found near the desk drawer that held the missing ledger.",
                    "Someone tampered with the hearing notes to remove references to missing foundation money."
                },
                interrogationQuestions = new[]
                {
                    "Where were you when Victor should have been preparing for the hearing?",
                    "Why were you personally concerned about the missing ledger?",
                    "What did you notice about the study or corridor that night?",
                    "Who do you think had the best chance to get close to Victor before the ledger vanished?"
                },
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Elena Voss",
                        portraitId = "FemCharacter1",
                        role = "Public relations advisor",
                        description = "Elena Voss presents herself like a woman who has survived too many scandals by never letting anyone see her panic. As the foundation's public relations advisor, she spent years polishing Victor's image while quietly resenting the way he treated everyone around him.",
                        appearance = "She is elegant, impeccably dressed, and carries herself with the practiced calm of someone used to hiding fear behind charm.",
                        connectionToCase = "She managed the foundation's public image and spent the evening trying to control how the coming scandal would look to the press.",
                        lastSeenVictim = "She claims she last saw Victor before dinner, when he brushed past her with the ledger under one arm and snapped that the press would hear only what he chose to reveal.",
                        motive = "Victor planned to dismiss her after the hearing, a humiliation that would have ended her carefully built reputation overnight.",
                        alibi = "She says she was in the ballroom making a tense last-minute call to the press office when the shouting began.",
                        personality = "Elegant, guarded, and quick to deflect",
                        openingStatement = "Victor liked keeping people on edge before a major announcement. I was dealing with the press when everything fell apart.",
                        interrogationAnswers = new[]
                        {
                            "I was finalizing press wording in the ballroom. I had no reason to be alone near the study.",
                            "Because the hearing would decide all our futures, including mine. Without the ledger, the whole foundation would look compromised.",
                            "I noticed movement in the corridor, but not enough to identify anyone. The study wing was tense long before the shouting began.",
                            "Nadia had the strongest reason to worry. She lived inside those records."
                        }
                    },
                    new Suspect
                    {
                        name = "Nadia Price",
                        portraitId = "FemCharacter2",
                        role = "Foundation accountant",
                        description = "Nadia Price is the sort of accountant who seems to disappear into the background until the numbers begin to turn dangerous. She had Victor's trust when it came to ledgers and budgets, but she also knew exactly how much ruin a single missing record could cause.",
                        appearance = "She is neat, composed, and severe, with the kind of precise posture that suggests she hates disorder in any form.",
                        connectionToCase = "She controlled the financial records and had been expected to walk Victor through the numbers one last time before sunrise.",
                        lastSeenVictim = "She says she last expected to see Victor in the study for their late-night review, but claims she reached the corridor only after he had already gone silent.",
                        motive = "The ledger could expose that she altered grant records and quietly hid stolen foundation funds behind tidy paperwork.",
                        alibi = "She says she went to find Victor after dinner when he missed their review and discovered the study already open.",
                        personality = "Controlled, precise, and slightly defensive",
                        openingStatement = "I was trying to prevent a public disaster, not cause one. Victor missed our review, so I went to find him.",
                        interrogationAnswers = new[]
                        {
                            "I left the accounting office after Victor failed to arrive for our review. That is when I went to the study wing.",
                            "Because the ledger anchored the hearing. Without it, Victor would be exposed and so would everyone connected to those numbers.",
                            "The study door was already open when I reached it, and the room looked disturbed. I stepped in to see if Victor had left in a hurry.",
                            "Marcus knew the lock schedule better than anyone, but Elena had plenty to lose from the hearing too."
                        }
                    },
                    new Suspect
                    {
                        name = "Marcus Flint",
                        portraitId = "MascCharacter2",
                        role = "Head of estate security",
                        description = "Marcus Flint looks like a man worn down by long nights, bad news, and employers who only notice security when it fails. He knew every lock, hallway, and blind angle in the estate, which makes his presence both useful and troubling.",
                        appearance = "He is broad-shouldered, tired-eyed, and carries himself like someone used to standing between rich people and their consequences.",
                        connectionToCase = "He oversaw the estate corridors, the study locks, and the guard movement at the exact hour the ledger disappeared.",
                        lastSeenVictim = "He says he last saw Victor heading into the study after dinner, locking the door behind him in the old habit he never broke.",
                        motive = "Victor had threatened to fire him after a string of recent security embarrassments at the estate.",
                        alibi = "He says he was outside checking the service gate after a brief power dip interrupted the house routine.",
                        personality = "Blunt, tired, but direct",
                        openingStatement = "Everyone points at security first, but I was dealing with the service gate when the lights flickered. That is exactly why I was away from the study.",
                        interrogationAnswers = new[]
                        {
                            "I was at the service gate during the power dip. When I came back in, the study corridor was already in chaos.",
                            "I was concerned because a missing ledger meant another failure pinned on my department, not because I wanted it gone.",
                            "The door had definitely been relocked after dinner. I know that because I checked the study wing myself earlier.",
                            "Nadia had the clearest access problem, and she was the one I saw leaving that corridor in a hurry."
                        }
                    }
                },
                guiltySuspect = "Nadia Price",
                guiltClues = new[]
                {
                    "Nadia says the study door was already open, but Marcus confirms it had been relocked after dinner.",
                    "The missing money trail gave Nadia the strongest personal reason to remove the ledger."
                },
                explanation = "Nadia Price stole the ledger to stop Victor from exposing the missing foundation money. Her slip about the study door being open reveals knowledge that only the culprit could have had after the room was relocked."
            };
        }

        private static CaseFile CreateMuseumSabotage()
        {
            return new CaseFile
            {
                caseTitle = "The Silent Gallery Sabotage",
                boardSummary = "A museum climate-control failure nearly ruined a major unveiling. The sabotage points toward someone who knew the restoration wing and cared too much about the transfer.",
                victimDescription = "Dr. Owen Vale was brilliant, vain, and far too willing to gamble with fragile things if applause waited on the other side. He had protégés who admired him, colleagues who despised him, and enough professional arrogance to make a desperate act feel almost inevitable.",
                crime = "Just before a high-profile unveiling, the climate control in the Ashbourne Museum restoration wing failed and sent the room into a dangerous swing of heat and moisture. Within minutes, Dr. Owen Vale's prized restoration project was on the brink of ruin and the evening dissolved into panic.",
                victim = "Dr. Owen Vale",
                victimPortraitId = "MascCharacter1",
                location = "Ashbourne Museum, restoration wing",
                onSiteClues = new[]
                {
                    "The emergency override panel was jammed with a brass palette knife from the conservator's tool set.",
                    "Only staff with restoration access could enter the control room without forcing the lock.",
                    "The sabotage happened minutes before the painting's transfer announcement."
                },
                interrogationQuestions = new[]
                {
                    "Where were you when the restoration alarms began?",
                    "Why did tonight's unveiling matter so much to you?",
                    "What do you know about the control room failure itself?",
                    "Who seemed most affected by Dr. Vale's decision to move forward with the transfer?"
                },
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Clara Benn",
                        portraitId = "FemCharacter1",
                        role = "Museum donor liaison",
                        description = "Clara Benn moves through a room like she was trained to smooth trouble out of the air before anyone important notices it. She is deeply invested in appearances, and an unveiling disaster would stain her reputation as much as the museum's.",
                        appearance = "She is polished, fashionably dressed, and carries a tense smile that never quite reaches her eyes once the evening begins to unravel.",
                        connectionToCase = "She managed the unveiling guests and spent the evening gliding between anxious donors, staff whispers, and the edge of the restoration wing.",
                        lastSeenVictim = "She says she last saw Dr. Vale charming donors near the reception arch before he disappeared back toward the restoration side of the museum.",
                        motive = "A failed unveiling would embarrass the donors she represented and leave her looking incompetent in front of the museum board.",
                        alibi = "She says she was escorting patrons back to the reception hall when staff rushed to seal the wing.",
                        personality = "Polished, anxious, and image-conscious",
                        openingStatement = "Tonight was supposed to be flawless. I was smoothing over donor nerves while someone turned the museum into a scandal.",
                        interrogationAnswers = new[]
                        {
                            "I was in the public hall escorting two patrons back to reception. I heard the alarms before I saw any staff running.",
                            "Because the unveiling was tied to donor trust. A disaster tonight lands on me too.",
                            "Only what I heard from the staff afterward. Everyone kept saying the control room had been compromised.",
                            "Mira took the transfer personally. She believed the painting should stay under her care."
                        }
                    },
                    new Suspect
                    {
                        name = "Mira Sloane",
                        portraitId = "FemCharacter2",
                        role = "Lead conservator",
                        description = "Mira Sloane speaks about the painting the way other people speak about family. Months of restoration had turned her protectiveness into obsession, and Dr. Vale's showmanship made that obsession dangerous.",
                        appearance = "She is poised and sharply observant, with careful hands and a stare that lingers longest on anyone treating the painting like a prop.",
                        connectionToCase = "She spent months restoring the painting and treated it less like museum property and more like something placed under her personal protection.",
                        lastSeenVictim = "She says she last saw Dr. Vale in a heated exchange outside the restoration room, where he dismissed her warnings about the transfer schedule.",
                        motive = "She believed Dr. Vale was gambling with the painting's safety for the sake of applause and prestige.",
                        alibi = "She says she was cataloguing solvents in the prep studio when the alarms erupted through the wing.",
                        personality = "Measured, proud, and quietly resentful",
                        openingStatement = "I cared more about that painting than anyone else in the building. Sabotaging the environment would mean attacking my own work.",
                        interrogationAnswers = new[]
                        {
                            "I was in the prep studio cataloguing solvents. By the time I came out, Vale and Jonah were already shouting.",
                            "Because the transfer was reckless. Vale was treating restoration like a stage trick.",
                            "Only rumors. The override panel was jammed, wasn't it? That is what people are whispering.",
                            "Vale's decision angered me, yes, but Jonah had to answer for the system failure too."
                        }
                    },
                    new Suspect
                    {
                        name = "Jonah Reed",
                        portraitId = "MascCharacter2",
                        role = "Night technician",
                        description = "Jonah Reed is nervous in the way a man becomes when one more mistake might cost him everything. He knows the museum systems better than anyone in the wing, but he also knows that makes him an easy target the moment something goes wrong.",
                        appearance = "He looks sleep-deprived and perpetually braced for blame, with grease on his sleeves and a habit of glancing toward every alarm before anyone else reacts.",
                        connectionToCase = "He monitored the museum systems and was one of the first people close enough to hear the restoration alarms from the moment they triggered.",
                        lastSeenVictim = "He says he last saw Dr. Vale earlier in the evening, complaining that maintenance delays were making him look foolish in front of donors.",
                        motive = "Dr. Vale had already filed complaints about Jonah's missed maintenance logs and made it clear another mistake could cost him his job.",
                        alibi = "He says he was tracing a lighting fault on the east staircase when the alarms began shrieking through the wing.",
                        personality = "Nervous, practical, and eager to be believed",
                        openingStatement = "I know the systems, sure, but that also means I knew how bad this was the second it started. I ran toward the alarms, not away from them.",
                        interrogationAnswers = new[]
                        {
                            "I was on the east staircase tracing a lighting fault. I reached the wing right after the alarms started.",
                            "It mattered because Vale had already been blaming me for every maintenance problem this month.",
                            "The control room door was half open when I got there, and the emergency override had been jammed on purpose.",
                            "Mira argued with Vale all week about the transfer. She took it far more personally than Clara ever did."
                        }
                    }
                },
                guiltySuspect = "Mira Sloane",
                guiltClues = new[]
                {
                    "Mira references the jammed override panel before that detail is publicly shared with her.",
                    "The brass palette knife came from the conservator's tool set that Mira used daily."
                },
                explanation = "Mira Sloane sabotaged the climate control to stop the transfer she believed would endanger the painting. Her answer shows inside knowledge of the jammed override panel before investigators should have revealed it."
            };
        }

        private static CaseFile CreateTheatreMurder()
        {
            return new CaseFile
            {
                caseTitle = "The Grand Lyric Dressing Room Murder",
                boardSummary = "A celebrated producer was found dead backstage on opening night. The theatre is packed with ambition, resentment, and one killer who knew exactly how to move unnoticed.",
                victimDescription = "Adrian Vale was a gifted producer with a savage ego and a habit of turning creative pressure into personal cruelty. He inspired devotion when the spotlight was bright, but behind the curtain he had spent years humiliating the very people who kept his productions alive.",
                crime = "Minutes before curtain, theatre producer Adrian Vale was found dead in a dressing room corridor at the Grand Lyric Theatre. Opening night froze in place as cast, crew, and staff were sealed backstage with the body and whatever secrets had just turned deadly.",
                victim = "Adrian Vale",
                victimPortraitId = "MascCharacter1",
                location = "Grand Lyric Theatre, backstage corridor",
                onSiteClues = new[]
                {
                    "A blood-specked cue sheet was folded into a lighting booth clipboard rather than left near the body.",
                    "The victim's missing signet ring was later found inside a costume repair tin.",
                    "Only staff with backstage access knew the corridor camera had failed during the five minutes before curtain."
                },
                interrogationQuestions = new[]
                {
                    "Where were you during the five minutes before curtain?",
                    "Why did Adrian Vale create pressure for you personally?",
                    "What did you notice about the backstage corridor after the commotion started?",
                    "Who had the best chance to move through backstage without drawing attention?"
                },
                suspects = new[]
                {
                    new Suspect
                    {
                        name = "Lena Morrell",
                        portraitId = "FemCharacter1",
                        role = "Stage manager",
                        description = "Lena Morrell runs backstage like a general holding a collapsing front line together by force of will alone. She knew Adrian's habits, his temper, and the terrible timing of every crisis he liked to create.",
                        appearance = "She is sharp-featured, brisk, and permanently on the edge of exhaustion, with the posture of someone who has not sat down all day.",
                        connectionToCase = "She controlled backstage traffic and coordinated the opening-night cues, making her one of the few people who always knew where everyone was supposed to be.",
                        lastSeenVictim = "She says she last saw Adrian storming down the corridor toward the dressing rooms after barking at two actors and blaming everyone for the night's nerves.",
                        motive = "Adrian had threatened to blame the entire evening on her if even the smallest detail slipped.",
                        alibi = "She says she was calling places from the cue desk when the shouting burst through backstage.",
                        personality = "Sharp, overworked, and direct",
                        openingStatement = "I needed the show to start on time, not collapse into panic. If Adrian died backstage, someone used the exact moment the theatre was at its busiest.",
                        interrogationAnswers = new[]
                        {
                            "I was at the cue desk calling places. Half the company can confirm they heard me over comms.",
                            "Because Adrian weaponized pressure. He loved making everyone feel replaceable, especially on opening night.",
                            "People flooded the corridor, but the lighting booth door was shut at first. That stood out because Noah usually kept it cracked.",
                            "Noah could disappear into the tech side of backstage without anyone thinking twice."
                        }
                    },
                    new Suspect
                    {
                        name = "Iris Bell",
                        portraitId = "FemCharacter2",
                        role = "Costume supervisor",
                        description = "Iris Bell has the protective fury of someone who has spent years stitching beauty together while others take credit for it. Adrian's contempt for her department had not made her forget a single slight.",
                        appearance = "She is immaculate despite the chaos around her, with pinned-back hair, quick hands, and the sharp gaze of someone always spotting what is out of place.",
                        connectionToCase = "She managed quick changes and had every excuse to move between dressing rooms, costume storage, and the narrow backstage corridors all evening.",
                        lastSeenVictim = "She says she last saw Adrian in passing near wardrobe, mocking the delay on a costume repair before striding toward the dressing-room corridor.",
                        motive = "Adrian had slashed her department budget and publicly humiliated her whenever wardrobe delays threatened his perfect production schedule.",
                        alibi = "She says she was repairing a torn hem in wardrobe storage when the corridor erupted in noise.",
                        personality = "Precise, protective, and simmering",
                        openingStatement = "I was trying to stop opening night from falling apart one costume at a time. Adrian enjoyed humiliating people, but I was busy fixing his chaos, not adding to it.",
                        interrogationAnswers = new[]
                        {
                            "I was in wardrobe storage fixing a hem. If you check the repair tin, my tools were there the whole time.",
                            "Adrian made my work miserable, but he did that to everyone. He cut corners and expected us to smile.",
                            "I noticed the corridor camera monitor was dark when people started yelling. I assumed tech was already dealing with it.",
                            "Lena knew everyone's movements, but Noah had the easiest way to vanish into a booth or catwalk."
                        }
                    },
                    new Suspect
                    {
                        name = "Noah Pike",
                        portraitId = "MascCharacter2",
                        role = "Lighting operator",
                        description = "Noah Pike watches the theatre from above, half hidden in shadow and machinery, which has made him observant and difficult to read. He noticed more than he admitted, and Adrian had given him every reason to turn bitterness into action.",
                        appearance = "He is lean, watchful, and unsmiling, with a technician's hands and the stillness of someone used to studying a room before entering it.",
                        connectionToCase = "He worked the lighting booth and catwalk above the backstage corridor, close enough to watch the chaos from above and slip out of sight when needed.",
                        lastSeenVictim = "He says he last saw Adrian below the booth, pacing the corridor alone and muttering about incompetence minutes before curtain.",
                        motive = "Adrian planned to replace him after recent technical mistakes and had made sure Noah knew his place was hanging by a thread.",
                        alibi = "He says he was finalizing lighting cues alone in the booth before curtain.",
                        personality = "Dry, observant, and hard to rattle",
                        openingStatement = "I watch people for a living, even when they forget that. Backstage chaos is still patterned chaos, and Adrian made plenty of enemies.",
                        interrogationAnswers = new[]
                        {
                            "I was in the lighting booth finalizing cues. Nobody came in because I was already behind.",
                            "Adrian wanted me gone after rehearsal mistakes. He made that very clear.",
                            "The corridor went wild fast, and somebody stuffed a cue sheet into my booth clipboard. Strange place to hide evidence unless they wanted it found later.",
                            "Iris moved all over backstage, but Lena had authority to go anywhere. That said, someone who knew about the dead corridor camera had a real edge."
                        }
                    }
                },
                guiltySuspect = "Noah Pike",
                guiltClues = new[]
                {
                    "The bloody cue sheet was hidden in Noah's lighting booth clipboard, linking the crime to his workspace.",
                    "Noah speaks casually about someone exploiting the failed corridor camera, knowledge limited to backstage technical staff."
                },
                explanation = "Noah Pike killed Adrian Vale to protect his job and then tried to bury the evidence inside the lighting booth clutter. His access to the failed camera window and the cue-sheet evidence tie the murder back to him."
            };
        }
    }
}
