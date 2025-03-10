using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel
{
    public class PanelPopup : PanelBase
    {
        [SerializeField] private Text txtMain;
        [SerializeField] private Button btnOk;
        [SerializeField] private Button btnCancel;

        private Action onClickOk;
        private Action onClickCancel;

        public override void Init()
        {
            if (isInit) return;
            isInit = true;
            btnOk.onClick.AddListener(OnClickOk);
            btnCancel.onClick.AddListener(OnClickCancel);
            btnOk.GetComponentInChildren<Text>().text = GetText("PanelPopupBtnOk");
            btnCancel.GetComponentInChildren<Text>().text = GetText("PanelPopupBtnCancel");
            Hide();
        }

        private void OnClickOk()
        {
            onClickOk?.Invoke();
            Hide();
        }

        private void OnClickCancel()
        {
            onClickCancel?.Invoke();
            Hide();
        }

        private void ShowBtnOk(bool isShow)
        {
            btnOk.gameObject.SetActive(isShow);
        }
        private void ShowBtnCancel(bool isShow)
        {
            btnCancel.gameObject.SetActive(isShow);
        }

        public void ShowPopup(string msg, Action callbackOk = null, Action callbackCancel = null)
        {
            txtMain.text = msg;
            onClickOk = callbackOk;
            onClickCancel = callbackCancel;
            ShowBtnOk(true);
            ShowBtnCancel(true);
            Show();
        }

    }
}