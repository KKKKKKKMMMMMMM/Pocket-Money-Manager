using KMUtils.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using KMUtils.Data.SortType;
using KMUtils.Manager;

namespace KMUtils.Panel
{
    public class TabSort : MonoBehaviour
    {
        private InterMain iMain;

        [SerializeField] private Button btnQuit;

        [SerializeField] private RectTransform[] rtDates;
        [SerializeField] private Dropdown[] ddSortDate;
        [SerializeField] private ItemTgl[] tglSortDate;
        [SerializeField] private ItemTgl[] tglSortType;
        [SerializeField] private ItemTgl[] tglSortOrder;
        [SerializeField] private ItemTgl[] tglSortCategory;
        [SerializeField] private TextLine[] txtLines;

        public Action fnHideCallback;

        private bool isInit = false;
        public void Init(InterMain inter)
        {
            if (isInit)
            {
                return;
            }
            isInit = true;
            iMain = inter;

            btnQuit.onClick.AddListener(HideSort);

            InitTextLine();
            InitDdSortDate();
            InitSortDate(); 
            InitSortType();
            InitSortOrder();
            InitCategory();
        }

        private void InitTextLine()
        {
            for (int i = 0; i < txtLines.Length; ++i)
            {
                txtLines[i].SetLine(iMain.GetText("TabSortTextLine" + i));
            }
        }

        private void InitDdSortDate()
        {
            ddSortDate[0].ClearOptions();
            ddSortDate[0].onValueChanged.RemoveAllListeners();
            ddSortDate[0].onValueChanged.AddListener(OnValueChangeDdYear);

            ddSortDate[1].ClearOptions();
            ddSortDate[1].onValueChanged.RemoveAllListeners();
            ddSortDate[1].onValueChanged.AddListener(OnValueChangeDdMonth);

            DateTime today = DateTime.Today;

            for (int i = today.Year - 5; i < today.Year + 6; ++i)
            {
                ddSortDate[0].options.Add(new Dropdown.OptionData($"{i}{iMain.GetText("Year")}"));
            }
            ddSortDate[0].value = 5;

            for (int i = 1; i < 13; ++i)
            {
                ddSortDate[1].options.Add(new Dropdown.OptionData($"{i}{iMain.GetText("Month")}"));
            }
            ddSortDate[1].value = today.Month - 1;
        }
        private void OnValueChangeDdYear(int value)
        {
            cDataManager.Instance.SetTargetYear(value + DateTime.Today.Year - 5);
        }
        private void OnValueChangeDdMonth(int value)
        {
            cDataManager.Instance.SetTargetMonth(value + 1);
        }

        private void InitSortDate()
        {
            string[] key = new string[]
            {
                "SortDate_Today",
                "SortDate_Month1",
                "SortDate_Month3",
                "SortDate_Month6",
                "SortDate_Total",
            };

            for (DateType type = DateType.Today; type <= DateType.All; ++type)
            {
                InitTglSortDate(type, iMain.GetText(key[(int)type]));
            }
            tglSortDate[(int)DateType.Month1].IsOn = true;
        }
        private void InitTglSortDate(DateType type, string str)
        {
            tglSortDate[(int)type].Init(str);
            tglSortDate[(int)type].onValueChanged = (isOn) =>
            {
                if (isOn)
                {
                    iMain.DataManager.ChangeSortType(type);
                }
            };
        }

        private void InitSortType()
        {
            string[] key = new string[]
            {
                "SortType_Total",
                "SortType_Input",
                "SortType_Output",
            };

            for (InOutType type = InOutType.Total; type <= InOutType.Output; ++type)
            {
                InitTglSortType(type, iMain.GetText(key[(int)type]));
            }
            tglSortType[(int)InOutType.Total].IsOn = true;
        }
        private void InitTglSortType(InOutType type, string str)
        {
            tglSortType[(int)type].Init(str);
            tglSortType[(int)type].onValueChanged = (isOn) =>
            {
                if (isOn)
                {
                    iMain.DataManager.ChangeSortType(type);
                }
            };
        }

        private void InitSortOrder()
        {
            string[] key = new string[]
            {
                "SortOrder_Date",
                "SortOrder_Money",
            };

            for (OrderType order = OrderType.Date; order <= OrderType.Money; ++order)
            {
                InitTglSortOrder(order, iMain.GetText(key[(int)order]));
            }
        }
        private void InitTglSortOrder(OrderType type, string str)
        {
            tglSortOrder[(int)type].Init(str);
            tglSortOrder[(int)type].onValueChanged = (isOn) =>
            {
                if (isOn)
                {
                    iMain.DataManager.ChangeSortType(type);
                }
            };
        }

        private void InitCategory()
        {
            for (int i = 0; i < tglSortCategory.Length; ++i)
            {
                InitTglSortCategory(i);
            }
        }
        private void InitTglSortCategory(int idx)
        {
            tglSortCategory[idx].Init();
            tglSortCategory[idx].IsOn = true;
            tglSortCategory[idx].onValueChanged = (isOn) =>
            {
                iMain.DataManager.SetShowCategory(idx, isOn);
            };
        }

        private void Refresh()
        {
            string[] cate = iMain.GetCategorys();
            for (int i = 0; i < tglSortCategory.Length; ++i)
            {
                if (i < cate.Length)
                {
                    tglSortCategory[i].SetText(cate[i]);
                    tglSortCategory[i].Show();
                }
                else
                {
                    tglSortCategory[i].Hide();
                }
            }
        }

        private void ShowDateDropdown(bool isShow)
        {
            rtDates[0].gameObject.SetActive(isShow);
            rtDates[1].gameObject.SetActive(!isShow);
        }

        public void ShowSortList()
        {
            ShowDateDropdown(false);
            Refresh();
            gameObject.SetActive(true);
        }
        public void ShowSortCalender()
        {
            ShowDateDropdown(true);
            Refresh();
            gameObject.SetActive(true);
        }

        public void HideSort()
        {
            fnHideCallback?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
