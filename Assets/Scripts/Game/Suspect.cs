using System;

namespace CaseFileLocalSuspect.Game
{
    [Serializable]
    public class Suspect
    {
        public string name;
        public string portraitId;
        public string role;
        public string description;
        public string appearance;
        public string connectionToCase;
        public string lastSeenVictim;
        public string motive;
        public string alibi;
        public string personality;
        public string openingStatement;
        public string[] interrogationAnswers;
    }
}
