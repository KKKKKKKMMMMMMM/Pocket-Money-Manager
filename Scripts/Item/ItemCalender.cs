using System;
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
        private Button btn;
        public Action<int, int, int> onClickBtn;
        private (int year, int month, int day) date;

        private void Awake()
        {
            btn = GetComponent<Button>();
        }

        public void Reset()
        {
            ActiveBtn(false);
            SetText();
            ShowRed(false);
            ShowBlue(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetDate(int year, int month, int day)
        {
            date = (year, month, day);
            SetText($"{date.day}");
        }

        private void SetText(string str = "")
        {
            txt.text = str;
        }

        public void ShowRed(bool isShow)
        {
            imgRed.gameObject.SetActive(isShow);
            if (isShow)
            {
                ActiveBtn(true);
            }
        }
        public void ShowBlue(bool isShow)
        {
            imgBlue.gameObject.SetActive(isShow);
            if (isShow)
            {
                ActiveBtn(true);
            }
        }

        private void ActiveBtn(bool isActive)
        {
            btn.enabled = isActive;
        }

        public void OnClickBtn()
        {
            onClickBtn?.Invoke(date.year, date.month, date.day);
        }
    }
}