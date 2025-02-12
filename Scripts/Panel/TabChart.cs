using KMUtils.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel.Chart
{
    public class TabChart : PanelBase
    {
        [SerializeField] private RectTransform rtNoData;
        [SerializeField] private ItemChart[] itemCharts;
        public Color32[] colors;

        public override void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;

            for (int i = 0; i < itemCharts.Length; ++i)
            {
                itemCharts[i].Init();
                itemCharts[i].SetColor(colors[i]);
                //itemCharts[i].Hide();
            }
        }

        public override void Show()
        {
            Refresh();
            base.Show();
        }

        public void Refresh()
        {
            KeyValuePair<string, int>[] group = cDataManager.Instance.GetChartData();
            if (group.Length > 0)
            {
                float max = group.Max(x => x.Value);
                for (int i = 0; i < itemCharts.Length; ++i)
                {
                    if (i < group.Length)
                    {
                        itemCharts[i].SetTxt(group[i].Key);
                        itemCharts[i].SetImg(group[i].Value / max);
                        itemCharts[i].Show();
                    }
                    else
                    {
                        itemCharts[i].SetTxt("");
                        itemCharts[i].Hide();
                    }
                }
                HideNodata();
            }
            else
            {
                ShowNodata();
            }
        }

        private void ShowNodata()
        {
            rtNoData.gameObject.SetActive(true);
        }
        private void HideNodata()
        {
            rtNoData.gameObject.SetActive(false);
        }
    }
}
