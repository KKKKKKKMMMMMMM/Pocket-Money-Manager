using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using KMUtils.Data;
using KMManager;
using KMUtils.Data.SortType;

namespace KMUtils.Manager
{
    public class cDataManager : Singleton<cDataManager>
    {
        private string directoryPath
        {
            get
            {
#if UNITY_EDITOR
                return $"{Application.dataPath}\\Resources\\Text";
#else
                return $"{Application.persistentDataPath}\\Resources\\Text";
#endif
            }
        }
        private string dataPath = "dataTable.txt";
        private string categoryPath = "categoryTable.txt";

        private List<cDataField> dataList;
        private Dictionary<string, string> txtMap;

        private bool isInit = false;
        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            if (dataList != null)
            {
                dataList.Clear();
            }

            if (txtMap != null)
            {
                txtMap.Clear();
            }
        }

        private void Init()
        {
            dataList = new List<cDataField>();
            txtMap = new Dictionary<string, string>();

            LoadText();
            LoadData();
            InitCategory();
            InitSort();

            isInit = true;
        }

        private void LoadText()
        {
            TextAsset ta = Resources.Load<TextAsset>("Text/txtTable");
            txtMap = ConvertManager.FromJson<Dictionary<string, string>>(ta.text);
        }

        public string GetText(string key)
        {
            if (txtMap.ContainsKey(key))
            {
                return txtMap[key];
            }
            else
            {
                return "#####";
            }
        }

        private void LoadData()
        {
            dataList = FileManager.LoadJson<List<cDataField>>(directoryPath, dataPath);
            if (dataList != null)
            {
                LogManager.Log($"LoadData {dataList.Count}");
            }
            else
            {
                dataList = new List<cDataField>();
            }
        }
        private void SaveData()
        {
            FileManager.SaveJson(directoryPath, dataPath, dataList);
        }

        public void AddData(DateTime date, string category, MoneyType type, int value, string info)
        {
            dataList.Add(new cDataField(date, category, type, value, info));
            SaveData();
        }
        public void AddData(cDataField data)
        {
            dataList.Add(data);
            SaveData();
        }

        public void AddData(DateTime date, int categoryNum, bool isIn, int value, string info)
        {
            cDataField data = new cDataField();
            data.date = date;
            data.category = dataCategory[categoryNum];
            data.type = isIn ? MoneyType.In : MoneyType.Out;
            data.value = value;
            data.info = info;
            dataList.Add(data);
            SaveData();
        }

        public void DeleteData(cDataField data)
        {
            if (dataList.Contains(data))
            {
                dataList.Remove(data);
                SaveData();
                LogManager.Log($"Delete Data Success");
            }
            else
            {
                LogManager.LogWarning($"Delete Data Error");
            }
        }

        public IEnumerable<cDataField> GetData()
        {
            return Sort(dataList);
        }

        public IEnumerable<cDataField> GetData(int year, int month, int day)
        {
            return dataList.Where(x => x.date.Year == year && x.date.Month == month && x.date.Day == day);
        }

        public IEnumerable<(string, int)> GetChartData()
        {
            return GetData()
                .Where(x => x.type == MoneyType.Out)
                .GroupBy(x => dataCategory.Contains(x.category) ? x.category : "Other")
                .Select(x => (x.Key, x.Sum(data => data.value)));
        }

        public IEnumerable<(int, int, int)> GetChartData(int categoryNum)
        {
            return GetData()
                .Where(x => x.type == MoneyType.Out)
                .Where(x => x.category == dataCategory[categoryNum])
                .GroupBy(x => (x.date.Year, x.date.Month))
                .Select(x => (x.Key.Year, x.Key.Month, x.Sum(data => data.value)));
        }

        #region Category

        private const int maxCategoryNum = 6;
        public int MaxCategoryNum
        {
            get
            {
                return maxCategoryNum;
            }
        }

        private string[] dataCategory;
        private bool[] isShowCategory;

        private void InitCategory()
        {
            LoadCategory();
        }

        private void LoadCategory()
        {
            SetCategorys(FileManager.LoadJson<string[]>(directoryPath, categoryPath));
        }

        private void SaveCategory()
        {
            if (dataCategory != null && dataCategory.Length > 0)
            {
                FileManager.SaveJson(directoryPath, categoryPath, dataCategory);
            }
        }

        public void SetCategorys(IEnumerable<string> data)
        {
            dataCategory = data?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? new string[1] { GetText("NoneCategory") };
            LogManager.Log($"DataManager SetCategory : {string.Join(" / ", dataCategory)}");
            isShowCategory = Enumerable.Repeat(true, MaxCategoryNum).ToArray();
            SaveCategory();
        }

        public string[] GetCategorys()
        {
            return dataCategory;
        }

#endregion

#region Sort Data

        [SerializeField] private SortType currSortType = SortType.Date;
        [SerializeField] private DateType currDateType = DateType.Month1;
        [SerializeField] private InOutType currInOutType = InOutType.Total;
        [SerializeField] private OrderType currOrderType = OrderType.Date;

        public SortType CurrSortType
        {
            get
            {
                return currSortType;
            }
        }

        public string GetSortText()
        {
            string ret;
            switch (currSortType)
            {
                case SortType.Date:

                    string dt;
                    switch(currDateType)
                    {
                        case DateType.Today:
                            dt = GetText("SortDate_Today");
                            break;
                        case DateType.Month1:
                            dt = GetText("SortDate_Month1");
                            break;
                        case DateType.Month3:
                            dt = GetText("SortDate_Month3");
                            break;
                        case DateType.Month6:
                            dt = GetText("SortDate_Month6");
                            break;
                        case DateType.All:
                            dt = GetText("SortDate_Total");
                            break;
                        default:
                            dt = "##";
                            break;
                    }

                    string io;
                    switch (currInOutType)
                    {
                        case InOutType.Total:
                            io = GetText("SortType_Total");
                            break;
                        case InOutType.Input:
                            io = GetText("SortType_Input");
                            break;
                        case InOutType.Output:
                            io = GetText("SortType_Output");
                            break;
                        default:
                            io = "##";
                            break;
                    }

                    string od;
                    switch(currOrderType)
                    {
                        case OrderType.Date:
                            od = GetText("SortOrder_Date");
                            break;
                        case OrderType.Money:
                            od = GetText("SortOrder_Money");
                            break;
                        default:
                            od = "##";
                            break;
                    }

                    ret = $"{dt}, {io}, {od}";
                    break;

                case SortType.Calender:
                    ret = string.Format(GetText("SortTextTargetMonth"), targetYear, targetMonth);
                    break;

                default:
                    ret = "####";
                    break;
            }
            return ret;
        }

        public void ChangeSortType(SortType type)
        {
            currSortType = type;
        }
        public void ChangeSortType(DateType type)
        {
            currDateType = type;
        }
        public void ChangeSortType(InOutType type)
        {
            currInOutType = type;
        }
        public void ChangeSortType(OrderType type)
        {
            currOrderType = type;
        }

        public void SetShowCategory(int index, bool isShow)
        {
            if (index < 0 || index >= isShowCategory.Length)
            {
                LogManager.LogError($"Error : ShowCategory {dataCategory.Length}, {isShowCategory.Length}, {index}, {isShow}");
                return;
            }
            isShowCategory[index] = isShow;
        }

        private int targetYear = 1999;
        private int targetMonth = 1;

        public int GetTargetYear()
        {
            return targetYear;
        }
        public int GetTargetMonth()
        {
            return targetMonth;
        }

        public void SetTargetYear(int year)
        {
            targetYear = year;
        }
        public void SetTargetMonth(int month)
        {
            targetMonth = month;
        }
        public void SetTargetDate(int year, int month)
        {
            targetYear = year;
            targetMonth = month;
        }

        private void InitSort()
        {
            DateTime today = DateTime.Today;
            targetYear = today.Year;
            targetMonth = today.Month;
        }

        public IEnumerable<cDataField> Sort(IEnumerable<cDataField> datas)
        {
            switch (currSortType)
            {
                case SortType.Date:
                    datas = SortByDate(datas);
                    break;

                case SortType.Calender:
                    datas = datas.Where(x => x.date.Date.Year == targetYear && x.date.Date.Month == targetMonth);
                    break;
            }

            datas = SortByType(datas);
            datas = SortByOrder(datas);
            datas = SortByCategory(datas);
            return datas;
        }

        private IEnumerable<cDataField> SortByDate(IEnumerable<cDataField> data)
        {
            DateTime min = DateTime.Today;
            DateTime max = DateTime.Today;
            switch (currDateType)
            {
                case DateType.Month1:
                    min = DateTime.Today.AddMonths(-1);
                    break;
                case DateType.Month3:
                    min = DateTime.Today.AddMonths(-3);
                    break;
                case DateType.Month6:
                    min = DateTime.Today.AddMonths(-6);
                    break;

                case DateType.Today:
                    break;

                case DateType.All:
                default:
                    return data;
            }
            return data.Where(x => min <= x.date.Date && x.date.Date <= max);
        }
        private IEnumerable<cDataField> SortByType(IEnumerable<cDataField> data)
        {
            switch (currInOutType)
            {
                case InOutType.Input:
                    data = data.Where(x => x.type == MoneyType.In);
                    break;
                case InOutType.Output:
                    data = data.Where(x => x.type == MoneyType.Out);
                    break;
            }
            return data;
        }
        private IEnumerable<cDataField> SortByOrder(IEnumerable<cDataField> data)
        {
            switch (currOrderType)
            {
                case OrderType.Date:
                    data = data.OrderByDescending(x => x.date);
                    break;
                case OrderType.Money:
                    data = data.OrderByDescending(x => x.value);
                    break;
            }
            return data;
        }
        private IEnumerable<cDataField> SortByCategory(IEnumerable<cDataField> data)
        {
            IEnumerable<string> hideCategory = dataCategory.Where((x, idx) => isShowCategory[idx] == false);
            return data.Where(x => hideCategory.Contains(x.category) == false);
        }
#endregion
    }
}