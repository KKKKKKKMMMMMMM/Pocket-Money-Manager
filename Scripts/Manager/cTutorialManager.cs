using KMUtils.Panel;
using KMUtils.Tutorial;
using KMUtils.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMUtils.Manager
{
    public class cTutorialManager : Singleton<cTutorialManager>
    {
        [SerializeField] private PanelHelp panelHelp;
        [SerializeField] private cTutorialPoint[] points;

        private string[] PointKey =
        {
            "Tutorial_List",
            "Tutorial_Add",
            "Tutorial_Category",
            "Tutorial_Sort",
        };

        private string GetText(string key)
        {
            return cDataManager.Instance.GetText(key);
        }

        private void StartTutorial(TutorialType type)
        {
            RectTransform[] target = points[(int)type].target;
            string key = PointKey[(int)type];
            for (int i = 0; i < target.Length; ++i)
            {
                panelHelp.AddQue(target[i], GetText(key + i));
            }
            panelHelp.StartQue();
        }

        public void ShowTutorial(TutorialType type)
        {
            switch (type)
            {
                case TutorialType.List:
                case TutorialType.Sort:
                case TutorialType.Add:
                    if (PlayerPrefs.GetInt(PointKey[(int)type], 0) == 0)
                    {
                        PlayerPrefs.SetInt(PointKey[(int)type], 1);
                        StartTutorial(type);
                    }
                    break;
            }
        }
    }
}