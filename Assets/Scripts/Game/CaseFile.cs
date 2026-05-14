using System;

namespace CaseFileLocalSuspect.Game
{
    [Serializable]
    public class CaseFile
    {
        public string caseTitle;
        public string crime;
        public string victim;
        public string location;
        public Suspect[] suspects;
        public string guiltySuspect;
        public string keyClue;
        public string explanation;
    }
}
