using KMUtils.Data;
using KMUtils.Manager;
using System;
using System.Collections.Generic;

namespace KMUtils
{
    public interface InterMain
    {
        public cDataManager DataManager { get; }

        public string GetSortText();

        public string GetText(string key);

        public string[] GetCategorys();
        public string GetCategory(int idx);

        public void SetCategory(string[] data);

        public IEnumerable<cDataField> GetData();

        public void AddData(cDataField data);
        public void AddData(DateTime date, string category, MoneyType type, int value, string info);

        public void OnQuit();
    }
}