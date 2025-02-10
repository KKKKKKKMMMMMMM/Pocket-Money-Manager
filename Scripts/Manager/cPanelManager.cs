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
        private PanelCalendar panelCalendar;
        private PanelChart panelChart;
        private PanelCategory panelCategory;

        private PanelType currPanel = PanelType.Main;

        private void Awake()
        {

        }

        public void Init(InterMain interMain)
        {
            panelMain = panels[(int)PanelType.Main].GetComponent<PanelMain>();
            panelList = panels[(int)PanelType.List].GetComponent<PanelList>();
            panelCalendar = panels[(int)PanelType.Calender].GetComponent<PanelCalendar>();
            panelChart = panels[(int)PanelType.Chart].GetComponent<PanelChart>();
            panelCategory = panels[(int)PanelType.Category].GetComponent<PanelCategory>();

            for (int i = 0; i < panels.Length; ++i)
            {
                panels[i].IMain = interMain;
                panels[i].IPanel = this;
                panels[i].Hide();
            }

            ShowPanel(PanelType.List);
            //panelLoading.EndCallback = EndLoading;
            //panelLoading.Show();
        }

        //private void EndLoading()
        //{
        //    ShowPanel(PanelType.List);
        //}

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
        public void ShowPanelCalendar()
        {
            ShowPanel(PanelType.Calender);
        }
        public void ShowPanelChart()
        {
            ShowPanel(PanelType.Chart);
        }
        public void ShowPanelAdd()
        {
            panels[(int)PanelType.Add].Show();
        }
        public void ShowPanelCategory()
        {
            panels[(int)PanelType.Category].Show();
        }
        #endregion
    }
}