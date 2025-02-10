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
    public class PanelAdd : PanelBase
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

        public override void Show()
        {
            Refresh();
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }

        public override void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;

            txtTitle.text = GetText("TitlePanelAdd");
            btnAdd.onClick.AddListener(AddInfo);
            btnQuit.onClick.AddListener(Hide);
            btnSetCategory.onClick.AddListener(iPanel.ShowPanelCategory);

            InitDropdown();
            InitCategory();
            InitType();
            InitTextLine();
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
            RefreshCategory();
        }
        private void InitItemTglCategory(int idx)
        {
            itemTglCategory[idx].Init();
            itemTglCategory[idx].onValueChanged = (isOn) =>
            {
                if (isOn)
                {
                    currCategory = idx;
                    Log($"OnToggleCategory {idx}");
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
            }
        }

        private void Refresh()
        {
            fields[(int)FieldType.Info].text = "";
            fields[(int)FieldType.Value].text = "";
            RefreshCategory();
        }
        private void RefreshCategory()
        {
            for (int i = 0; i < itemTglCategory.Length; ++i)
            {
                itemTglCategory[i].Hide();
            }

            string[] data = cDataManager.Instance.GetCategorys();
            for (int i = 0; i < data.Length; ++i)
            {
                itemTglCategory[i].SetText(data[i]);
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

        private IEnumerator ShowEdgeRed(int idx)
        {
            imgEdgeRed[idx].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            imgEdgeRed[idx].gameObject.SetActive(false);
        }


    }
}
