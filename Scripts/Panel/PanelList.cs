using KMUtils.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Data.SortType;
using KMUtils.Manager;
using System;
using KMUtils.Type;

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
        [SerializeField] private PanelAdd panelAdd;
        [SerializeField] private RectTransform imgNoData;
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
            if (isInit) return;
            isInit = true;

            InitBtn();
            InitText();
            InitItem();
            InitTabCalender();

            panelAdd.Init();
            panelAdd.addCallback = Refresh;
            tabSort.Init(iMain);
            tabSort.fnHideCallback = Refresh;
            tabCalender.showTargetDayList = ShowTargetDayList;
            tabCalender.IMain = iMain;
            tabCalender.IPanel = iPanel;
            tabChart.IMain = iMain;
            tabChart.IPanel = iPanel;
            tabChart.Init();

            ShowList();
        }

        private void InitBtn()
        {
            btnQuit.onClick.AddListener(iMain.OnQuit);
            btnAdd.onClick.AddListener(ShowAdd);
            btnChange.onClick.AddListener(ChangeTab);
            btnSort.onClick.AddListener(ShowSort);
            btnChart.onClick.AddListener(OnClickTabChart);
        }

        private void InitText()
        {
            string[] key = new string[]
            {
                "RemainingMoney",
                "InputMoney",
                "OutputMoney"
            };
            for (int i = 0; i < totals.Length; ++i)
            {
                txtTotals[i].text = $"{GetText(key[i])} : {totals[i]}{GetText("MoneyType")}";
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
                item.onClickCallback = OnClickItem;
                itemPool.Add(item);
            }
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

        private void InitTabCalender()
        {
            tabCalender.IMain = iMain;
            tabCalender.IPanel = IPanel;
            tabCalender.Init();
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
            imgNoData.gameObject.SetActive(datas.Length < 1);

            for (int i = 0; i < datas.Length; ++i)
            {
                itemPool[i].Show(datas[i]);
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

        private void ShowSort()
        {
            switch (currTab)
            {
                case Tab.List:
                    tabSort.ShowSortList();
                    break;

                case Tab.Calender:
                    tabSort.ShowSortCalender();
                    break;
            }
            cTutorialManager.Instance.ShowTutorial(TutorialType.Sort);
        }

        private void ShowAdd()
        {
            panelAdd.Show();
            cTutorialManager.Instance.ShowTutorial(TutorialType.Add);
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
            cDataManager.Instance.ChangeSortType(SortType.Date);
            Refresh();
            tabList.gameObject.SetActive(true);
            tabCalender.Hide();
        }

        private void ShowCalender()
        {
            cDataManager.Instance.ChangeSortType(SortType.Calender);
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

        private void OnClickItem(cDataField data)
        {
            string msg = $"{GetText("Date")} : {data.date:yy.MM.dd}\n"
                       + $"{GetText("Info")} : {data.info}\n"
                       + $"{GetText("Value")} : {data.value}\n"
                       + GetText("PanelListDeleteMsg");

            iPanel.ShowPopup(msg, () =>
            {
                cDataManager.Instance.DeleteData(data);
                Refresh();
            });
        }

        private void ShowTargetDayList(int year, int month, int day)
        {
            cDataField[] datas = cDataManager.Instance.GetData(year, month, day).ToArray();
            RefreshData(datas);
            RefreshTotal(datas);
            txtSort.text = $"{year:0000}.{month:00}.{day:00}";
            tabList.gameObject.SetActive(true);
            tabCalender.Hide();
        }
    }
}