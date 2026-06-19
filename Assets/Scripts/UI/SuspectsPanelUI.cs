using CaseFileLocalSuspect.Game;
using UnityEngine.UI;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class SuspectsPanelUI : MonoBehaviour
    {
        [SerializeField] private SuspectCardUI suspectCard;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        private CaseFile currentCaseFile;
        private PortraitLibrary currentPortraitLibrary;
        private int selectedSuspectIndex;

        public void ShowSuspects(CaseFile caseFile, PortraitLibrary portraitLibrary)
        {
            if (currentCaseFile != caseFile)
            {
                selectedSuspectIndex = 0;
            }

            currentCaseFile = caseFile;
            currentPortraitLibrary = portraitLibrary;
            selectedSuspectIndex = Mathf.Clamp(selectedSuspectIndex, 0, GetLastSuspectIndex());
            RefreshView();
        }

        public void ShowPreviousSuspect()
        {
            if (selectedSuspectIndex <= 0)
            {
                return;
            }

            selectedSuspectIndex--;
            RefreshView();
        }

        public void ShowNextSuspect()
        {
            if (selectedSuspectIndex >= GetLastSuspectIndex())
            {
                return;
            }

            selectedSuspectIndex++;
            RefreshView();
        }

        private void RefreshView()
        {
            if (suspectCard == null)
            {
                return;
            }

            Suspect selectedSuspect = currentCaseFile != null
                && currentCaseFile.suspects != null
                && currentCaseFile.suspects.Length > 0
                && selectedSuspectIndex >= 0
                && selectedSuspectIndex < currentCaseFile.suspects.Length
                ? currentCaseFile.suspects[selectedSuspectIndex]
                : null;
            Sprite selectedPortrait = currentPortraitLibrary != null && selectedSuspect != null
                ? currentPortraitLibrary.GetPortrait(selectedSuspect.portraitId)
                : null;
            suspectCard.SetSuspect(selectedSuspect, selectedPortrait);
            if (previousButton != null)
            {
                previousButton.interactable = selectedSuspectIndex > 0;
            }

            if (nextButton != null)
            {
                nextButton.interactable = selectedSuspectIndex < GetLastSuspectIndex();
            }
        }

        private int GetLastSuspectIndex()
        {
            return currentCaseFile != null && currentCaseFile.suspects != null && currentCaseFile.suspects.Length > 0
                ? currentCaseFile.suspects.Length - 1
                : 0;
        }
    }
}
