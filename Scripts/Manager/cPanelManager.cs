using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMUtils.Panel;
using KMUtils.Type;
using KMManager;
using System;

namespace KMUtils.Manager
{
    public class cPanelManager : Singleton<cPanelManager>, InterPanel
    {
        [SerializeField] private PanelBase[] panels;

        private PanelMain panelMain;
        private PanelList panelList;
        private PanelPopup panelPopup;
        private PanelHelp panelHelp;

        private PanelType currPanel = PanelType.Main;

        private void Awake()
        {

        }

        public void Init(InterMain interMain)
        {
            panelMain = panels[(int)PanelType.Main].GetComponent<PanelMain>();
            panelList = panels[(int)PanelType.List].GetComponent<PanelList>();
            panelPopup = panels[(int)PanelType.Popup].GetComponent<PanelPopup>();
            panelHelp = panels[(int)PanelType.Help].GetComponent<PanelHelp>();

            for (int i = 0; i < panels.Length; ++i)
            {
                panels[i].IMain = interMain;
                panels[i].IPanel = this;
                panels[i].Init();
                panels[i].Hide();
            }

            ShowPanel(PanelType.List);
            cTutorialManager.Instance.ShowTutorial(TutorialType.List);
        }

        public void ShowPanel(PanelType type)
        {
            panels[(int)currPanel].Hide();
            currPanel = type;
            panels[(int)currPanel].Show();
        }

        #region IPanel

        public void ShowPanelMain()
        {
            ShowPanel(PanelType.Main);
        }
        public void ShowPanelList()
        {
            ShowPanel(PanelType.List);
        }

        public void ShowPopup(string msg, Action callbackok = null, Action callbackcancel = null)
        {
            panelPopup.ShowPopup(msg, callbackok, callbackcancel);
        }
        #endregion
    }
}