//#define LogPanelAdd

using KMUtils.Data;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Manager;

namespace KMUtils.Panel
{
    public class PanelAdd : MonoBehaviour
    {
        [SerializeField] private Button btnQuit;
        [SerializeField] private Button btnAdd;

        [SerializeField] private Button btnSetCategory;

        [SerializeField] private Text txtTitle;
        [SerializeField] private Dropdown[] drops;
        [SerializeField] private InputField[] fields;
        [SerializeField] private ItemTgl[] itemTglCategory;
        [SerializeField] private ItemTgl[] tglTypes;

        [SerializeField] private TextLine[] txtLines;

        [SerializeField] private Image[] imgEdgeRed;

        [SerializeField] private PanelCategory panelCategory;

        public Action addCallback;

        private const int YearRange = 10;

        private int[] yearArray;
        private int currYear = 1999;
        private int currMonth = 1;
        private int currDay = 1;
        private int prevDay = 0;

        private int currCategory = 0;

        private enum DropType
        {
            Year,
            Month,
            Day,
        }

        private enum FieldType
        {
            Info,
            Value,
        }

        private void OnDestroy()
        {

        }

        public void Show()
        {
            Refresh();
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private string GetText(string key)
        {
            return cDataManager.Instance.GetText(key);
        }

        private bool isInit = false;
        public void Init()
        {
            if (isInit) return;
            isInit = true;

            txtTitle.text = GetText("TitlePanelAdd");
            btnAdd.onClick.AddListener(AddInfo);
            btnQuit.onClick.AddListener(Hide);
            btnSetCategory.onClick.AddListener(()=>panelCategory.Show(cDataManager.Instance.GetCategorys()));

            InitDropdown();
            InitCategory();
            InitType();
            InitTextLine();

            panelCategory.Init(HideCategory);

            Refresh();
        }

        private void InitDropdown()
        {
            DateTime today = DateTime.Today;
            int day = DateTime.DaysInMonth(today.Year, today.Month);
            yearArray = Enumerable.Range(today.Year - YearRange, YearRange * 2 + 1).ToArray();
            drops[(int)DropType.Year].onValueChanged.AddListener(RefreshDropdownDay_Year);
            drops[(int)DropType.Month].onValueChanged.AddListener(RefreshDropdownDay_Month);
            drops[(int)DropType.Day].onValueChanged.AddListener(RefreshDropdownDay_Day);
            SetDropDownOption(drops[(int)DropType.Year], yearArray.Select(x => $"{x:0000}"), YearRange);
            SetDropDownOption(drops[(int)DropType.Month], Enumerable.Range(1, 12).Select(x => $"{x:00}"), today.Month - 1);
            drops[(int)DropType.Day].value = today.Day - 1;
        }

        private void SetDropDownOption(Dropdown Dd, IEnumerable<string> options, int value = 0)
        {
            Dd.options.Clear();
            Dd.AddOptions(options.ToList());
            Dd.value = value;
        }

        private void InitCategory()
        {
            for (int i = 0; i < itemTglCategory.Length; ++i)
            {
                InitItemTglCategory(i);
            }
        }
        private void InitItemTglCategory(int idx)
        {
            itemTglCategory[idx].Init();
            itemTglCategory[idx].onValueChanged = (isOn) =>
            {
                if (isOn)
                {
                    currCategory = idx;
                }
            };
        }

        private void InitType()
        {
            string[] key = new string[]
            {
                "InputMoney",
                "OutputMoney",
            };

            for (int i = 0; i < key.Length; ++i)
            {
                tglTypes[i].Init(GetText(key[i]));
            }
        }

        private void InitTextLine()
        {
            for (int i = 0; i < txtLines.Length; ++i)
            {
                txtLines[i].SetLine(GetText("PanelAddTextLine" + i));
            }
        }

        public void AddInfo()
        {
            DateTime date = new DateTime(currYear, currMonth, currDay);
            if (CheckInputField(FieldType.Info) &&
                CheckInputField(FieldType.Value))
            {
                string info = fields[(int)FieldType.Info].text;
                int value = Convert.ToInt32(fields[(int)FieldType.Value].text);
                cDataManager.Instance.AddData(date, currCategory, tglTypes[0].IsOn, value, info);
                Hide();
                addCallback?.Invoke();
            }
        }

        private void Refresh()
        {
            fields[(int)FieldType.Info].text = "";
            fields[(int)FieldType.Value].text = "";
            RefreshCategory(cDataManager.Instance.GetCategorys());
        }
        private void RefreshCategory(string[] category)
        {
            for (int i = 0; i < itemTglCategory.Length; ++i)
            {
                itemTglCategory[i].Hide();
            }

            for (int i = 0; i < category.Length; ++i)
            {
                itemTglCategory[i].SetText(category[i]);
                itemTglCategory[i].Show();
            }
        }

        private void RefreshDropdownDay_Year(int value)
        {
            currYear = yearArray[value];
            RefreshDropdownDay(currYear, currMonth);
        }
        private void RefreshDropdownDay_Month(int value)
        {
            currMonth = value + 1;
            RefreshDropdownDay(currYear, currMonth);
        }
        private void RefreshDropdownDay_Day(int value)
        {
            currDay = value + 1;
        }
        private void RefreshDropdownDay(int year, int month)
        {
            int day = DateTime.DaysInMonth(year, month);
            if (day != prevDay)
            {
                prevDay = day;
                SetDropDownOption(drops[(int)DropType.Day], Enumerable.Range(1, day).Select(x => $"{x:00}"));
            }
        }

        private bool CheckInputField(FieldType type)
        {
            int idx = (int)type;
            if (string.IsNullOrWhiteSpace(fields[idx].text))
            {
                if (imgEdgeRed[idx].gameObject.activeSelf == false)
                {
                    StartCoroutine(ShowEdgeRed(idx));
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private WaitForSeconds wait = new WaitForSeconds(1f);
        private IEnumerator ShowEdgeRed(int idx)
        {
            imgEdgeRed[idx].gameObject.SetActive(true);
            yield return wait;
            imgEdgeRed[idx].gameObject.SetActive(false);
        }

        private void HideCategory(string[] data)
        {
            cDataManager.Instance.SetCategorys(data);
            Refresh();
        }
    }
}
