using KMUtils.Data;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Manager;

namespace KMUtils.Panel.Calender
{
    public class TabCalender : PanelBase
    {
        [SerializeField] private Text[] txtDayOfWeek;
        [SerializeField] private RectTransform rtCalender;
        [SerializeField] private GameObject preCalender;

        private List<ItemCalender> itemCalenders;
        public Action<int, int, int> showTargetDayList;

        private void Start()
        {

        }

        private void OnDestroy()
        {
            if (itemCalenders != null)
            {
                foreach(ItemCalender item in itemCalenders)
                {
                    DestroyImmediate(item);
                }
                itemCalenders = null;
            }
        }

        public override void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;

            InitDayOfWeek();
            InitCalender();
        }

        private void InitDayOfWeek()
        {
            for (int i = 0; i < txtDayOfWeek.Length; ++i)
            {
                txtDayOfWeek[i].text = iMain.GetText("DayOfWeek" + i);
            }
        }

        private void InitCalender()
        {
            int max = 7 * 6;
            itemCalenders = new List<ItemCalender>();
            for (int i = 0; i < max; ++i)
            {
                GameObject obj = Instantiate(preCalender);
                obj.transform.SetParent(rtCalender);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.name = "Calender" + i;
                ItemCalender item = obj.GetComponent<ItemCalender>();
                item.Reset();
                item.onClickBtn = OnClickItemCalender;
                itemCalenders.Add(item);
            }
        }


        public override void Show()
        {
            Refresh();
            base.Show();
        }

        public void Refresh()
        {
            int year = iMain.DataManager.GetTargetYear();
            int month = iMain.DataManager.GetTargetMonth();

            for (int i = 0; i < itemCalenders.Count; ++i)
            {
                itemCalenders[i].Reset();
            }

            Dictionary<int, (bool isIn, bool isOut)> dic = cDataManager.Instance.GetData()
                .GroupBy(x => x.date.Day)
                .ToDictionary(x => x.Key, (x => (x.Any(d => d.type == MoneyType.In), x.Any(d => d.type == MoneyType.Out))));

            int dow = (int)new DateTime(year, month, 1).DayOfWeek;
            int last = DateTime.DaysInMonth(year, month) + dow;
            int day = 1;
            for (int i = dow; i < last; ++i)
            {
                itemCalenders[i].SetDate(year, month, day);
                if (dic.ContainsKey(day))
                {
                    itemCalenders[i].ShowBlue(dic[day].isIn);
                    itemCalenders[i].ShowRed(dic[day].isOut);
                }
                ++day;
            }

            Action<ItemCalender> action = (last <= 7 * 5) ? (item => item.Hide()) : (item => item.Show());
            for (int i = 7 * 5; i < itemCalenders.Count; ++i)
            {
                action(itemCalenders[i]);
            }
        }

        private void OnClickItemCalender(int year, int month, int day)
        {
            LogManager.Log($"OnClick {year} - {month} - {day}");
            showTargetDayList?.Invoke(year, month, day);
        }
    }
}