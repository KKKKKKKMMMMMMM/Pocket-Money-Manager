using KMUtils.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Panel.Calender;
using KMUtils.Data.SortType;
using KMUtils.Panel.Chart;

namespace KMUtils.Panel
{
    public class PanelList : PanelBase
    {
        [SerializeField] private Button btnQuit;
        [SerializeField] private Button btnSort;
        [SerializeField] private Button btnChange;
        [SerializeField] private Button btnAdd;
        [SerializeField] private Button btnChart;

        [SerializeField] private TabSort tabSort;
        [SerializeField] private RectTransform tabList;
        [SerializeField] private TabCalender tabCalender;
        [SerializeField] private TabChart tabChart;
        [SerializeField] private Text txtSort;
        [SerializeField] private Text[] txtTotals;

        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private GameObject itemPrefab;

        private int[] totals = new int[] { 0, 0, 0 };
        private const int ItemMax = 30;
        private List<ItemDFList> itemPool;

        private void OnDestroy()
        {
            DestroyItem();
        }

        public override void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;

            btnQuit.onClick.AddListener(iMain.OnQuit);
            btnAdd.onClick.AddListener(iPanel.ShowPanelAdd);
            btnChange.onClick.AddListener(ChangeTab);
            btnSort.onClick.AddListener(ShowSort);
            btnChart.onClick.AddListener(OnClickTabChart);
            InitText();
            InitItem();
            InitTabCalender();
            tabSort.Init(iMain);
            tabSort.fnHideCallback = Refresh;

            tabCalender.IMain = iMain;
            tabCalender.IPanel = iPanel;
            tabChart.IMain = iMain;
            tabChart.IPanel = iPanel;
            tabChart.Init();

            ShowList();
        }

        private void InitText()
        {
            string[] key = new string[]
            {
                "잔액 : ",
                "입금 : ",
                "출금 : "
            };
            for (int i = 0; i < totals.Length; ++i)
            {
                txtTotals[i].text = $"{key[i]}{totals[i]}원";
            }
        }
        private void InitItem()
        {
            itemPool = new List<ItemDFList>();            
            for (int i = 0; i < ItemMax; ++i)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.SetParent(scrollView.content);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.name = $"item{i + 1}";
                ItemDFList item = obj.GetComponent<ItemDFList>();
                item.Hide();
                itemPool.Add(item);
            }
        }

        private void InitTabCalender()
        {
            tabCalender.IMain = iMain;
            tabCalender.IPanel = IPanel;
            tabCalender.Init();
        }

        private void DestroyItem()
        {
            if (itemPool != null && itemPool.Count > 0)
            {
                foreach (ItemDFList item in itemPool)
                {
                    DestroyImmediate(item);
                }
            }
        }

        public override void Show()
        {
            base.Show();
            Refresh();
        }
        public override void Hide()
        {
            base.Hide();
        }

        public void Refresh()
        {
            cDataField[] datas = iMain.GetData().ToArray();
            if (currTab == Tab.List)
            {
                RefreshData(datas);
            }
            else
            {
                tabCalender.Refresh();
            }
            RefreshTotal(datas);          
            RefreshTxtSort();
        }

        private void RefreshData(cDataField[] datas)
        {
            int lastValue = 0;
            for (int i = 0; i < datas.Length; ++i)
            {
                lastValue += datas[i].type == MoneyType.In ? datas[i].value : -datas[i].value;
                itemPool[i].Show(GetStrData(datas[i], lastValue));
            }

            if (datas.Length >= ItemMax)
            {
                return;
            }

            for (int i = datas.Length; i < ItemMax; ++i)
            {
                itemPool[i].Hide();
            }
        }

        private void RefreshTotal(cDataField[] datas)
        {
            int totalIn = datas.Where(x => x.type == MoneyType.In).Select(x => x.value).Sum();
            int totalOut = datas.Where(x => x.type == MoneyType.Out).Select(x => x.value).Sum();
            txtTotals[0].text = $"{GetText("RemainingMoney")} {totalIn - totalOut}{GetText("MoneyType")}";
            txtTotals[1].text = $"{GetText("InputMoney")} {totalIn}{GetText("MoneyType")}";
            txtTotals[2].text = $"{GetText("OutputMoney")} {totalOut}{GetText("MoneyType")}";
        }

        public string[] GetStrData(cDataField data, int lastValue)
        {
            string[] strdata = new string[5];
            string dow = GetText($"DayOfWeek{(int)data.date.DayOfWeek}");
            strdata[0] = $"{data.date.Year:0000}-{data.date.Month:00}-{data.date.Day:00} ({dow})";
            strdata[1] = $"{data.category}";
            if (data.type == MoneyType.In)
            {
                strdata[2] = $"{GetText("InputMoney")} <color=blue>{data.value}</color>{GetText("MoneyType")}";
            }
            else
            {
                strdata[2] = $"{GetText("OutputMoney")} <color=red>{data.value}</color>{GetText("MoneyType")}";
            }
            strdata[3] = $"{GetText("RemainingMoney")} {lastValue}{GetText("MoneyType")}";
            strdata[4] = $"{data.info}";

            return strdata;
        }

        private void ShowSort()
        {
            switch(currTab)
            {
                case Tab.List:
                    tabSort.ShowSortList();
                    break;

                case Tab.Calender:
                    tabSort.ShowSortCalender();
                    break;
            }
        }

        private enum Tab
        {
            List,
            Calender,
        }
        private Tab currTab = Tab.List;

        private void ChangeTab()
        {
            if (tabChart.gameObject.activeSelf)
            {
                tabChart.Hide();
            }

            switch (currTab)
            {
                case Tab.List:
                    currTab = Tab.Calender;
                    ShowCalender();
                    break;

                case Tab.Calender:
                    currTab = Tab.List;
                    ShowList();
                    break;
            }
        }

        private void ShowList()
        {
            iMain.DataManager.ChangeSortType(SortType.Date);
            Refresh();
            tabList.gameObject.SetActive(true);
            tabCalender.Hide(); 
        }

        private void ShowCalender()
        {
            iMain.DataManager.ChangeSortType(SortType.Calender);
            Refresh();
            tabList.gameObject.SetActive(false);
            tabCalender.Show();
            RefreshTxtSort();
        }

        private void RefreshTxtSort()
        {
            txtSort.text = iMain.GetSortText();
        }

        private void OnClickTabChart()
        {
            if (tabChart.gameObject.activeSelf)
            {
                tabChart.Hide();
            }
            else
            {
                tabChart.Show();
            }
        }
    }
}