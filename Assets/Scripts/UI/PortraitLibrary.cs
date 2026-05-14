using System;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class PortraitLibrary : MonoBehaviour
    {
        [Serializable]
        public class PortraitEntry
        {
            public string id;
            public Sprite sprite;
        }

        [SerializeField] private PortraitEntry[] portraits;

        public Sprite GetPortrait(string portraitId)
        {
            if (portraits == null || string.IsNullOrWhiteSpace(portraitId))
            {
                return null;
            }

            for (int i = 0; i < portraits.Length; i++)
            {
                if (string.Equals(portraits[i].id, portraitId, StringComparison.OrdinalIgnoreCase))
                {
                    return portraits[i].sprite;
                }
            }

            return null;
        }
    }
}
