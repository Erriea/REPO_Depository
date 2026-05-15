using System;

namespace CaseFileLocalSuspect.Game
{
    [Serializable]
    public class Suspect
    {
        public string name;
        public string portraitId;
        public string role;
        public string connectionToCase;
        public string motive;
        public string alibi;
        public string personality;
        public string openingStatement;
        public FollowUpQuestion[] followUpQuestions;
    }
}
