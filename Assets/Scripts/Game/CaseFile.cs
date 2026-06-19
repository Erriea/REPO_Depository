using System;

namespace CaseFileLocalSuspect.Game
{
    [Serializable]
    public class CaseFile
    {
        public string caseTitle;
        public string boardSummary;
        public string victimDescription;
        public string crime;
        public string victim;
        public string victimPortraitId;
        public string location;
        public string[] onSiteClues;
        public Suspect[] suspects;
        public string[] interrogationQuestions;
        public string guiltySuspect;
        public string[] guiltClues;
        public string explanation;
    }
}
