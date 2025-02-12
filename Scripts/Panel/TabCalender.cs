using KMUtils.Data;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel.Calender
{
    public class TabCalender : PanelBase
    {
        [SerializeField] private Text[] txtDayOfWeek;
        [SerializeField] private RectTransform rtCalender;
        [SerializeField] private GameObject preCalender;

        private List<ItemCalender> itemCalenders;

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
            Refresh(year, month);
        }

        private void Refresh(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            int dow = (int)date.DayOfWeek;
            IEnumerable<cDataField> datas = iMain.GetData().Where(x => x.date.Year == year && x.date.Month == month);
            for (int i = 0; i < itemCalenders.Count; ++i)
            {
                itemCalenders[i].Reset();
            }
            int last = DateTime.DaysInMonth(year, month) + dow;
            int cnt = 1;
            for (int i = dow; i < last; ++i)
            {
                itemCalenders[i].SetText($"{cnt}");
                if(datas.Any(x => x.date.Day == cnt))
                {
                    if (datas.Any(x => x.type == MoneyType.In))
                    {
                        itemCalenders[i].ShowBlue();
                    }
                    if (datas.Any(x => x.type == MoneyType.Out))
                    {
                        itemCalenders[i].ShowRed();
                    }
                }
                ++cnt;
            }

            if (last <= 7 * 5)
            {
                for (int i = 7 * 5; i < itemCalenders.Count; ++i)
                {
                    itemCalenders[i].Hide();
                }
            }
            else
            {
                for (int i = 7 * 5; i < itemCalenders.Count; ++i)
                {
                    itemCalenders[i].Show();
                }
            }
        }
    }
}