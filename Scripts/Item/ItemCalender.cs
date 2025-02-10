using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel.Calender
{
    public class ItemCalender : MonoBehaviour
    {
        [SerializeField] private Text txt;
        [SerializeField] private Image imgRed;
        [SerializeField] private Image imgBlue;

        public void Reset()
        {
            SetText("");
            HideRed();
            HideBlue();
        }

        public string GetText()
        {
            return txt.text;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetText(string str)
        {
            txt.text = str;
        }

        public void ShowRed()
        {
            imgRed.gameObject.SetActive(true);
        }
        public void HideRed()
        {
            imgRed.gameObject.SetActive(false);
        }

        public void ShowBlue()
        {
            imgBlue.gameObject.SetActive(true);
        }
        public void HideBlue()
        {
            imgBlue.gameObject.SetActive(false);
        }
    }
}