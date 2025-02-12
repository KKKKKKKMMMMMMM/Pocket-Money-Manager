using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMUtils.Panel;
using KMUtils.Type;
using KMManager;

namespace KMUtils.Manager
{
    public class cPanelManager : MonoBehaviour, InterPanel
    {
        [SerializeField] private PanelBase[] panels;

        private PanelMain panelMain;
        private PanelList panelList;

        private PanelType currPanel = PanelType.Main;

        private void Awake()
        {

        }

        public void Init(InterMain interMain)
        {
            panelMain = panels[(int)PanelType.Main].GetComponent<PanelMain>();
            panelList = panels[(int)PanelType.List].GetComponent<PanelList>();

            for (int i = 0; i < panels.Length; ++i)
            {
                panels[i].IMain = interMain;
                panels[i].IPanel = this;
                panels[i].Hide();
            }

            ShowPanel(PanelType.List);
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
        public void ShowPanelAdd()
        {
            panels[(int)PanelType.Add].Show();
        }
        #endregion
    }
}