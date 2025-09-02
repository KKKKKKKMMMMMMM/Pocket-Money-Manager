using KMUtils.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel
{
    public class TabChart : PanelBase
    {
        [SerializeField] private Text txtTitle;
        [SerializeField] private RectTransform rtNoData;
        [SerializeField] private ItemChart[] itemCharts;
        public Color32[] colors;

        private enum ChartType
        {
            All,
            TargetCategory,
        }
        private ChartType currChartType = ChartType.All;

        public override void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;

            for (int i = 0; i < itemCharts.Length; ++i)
            {
                InitItemChart(i);
            }
        }

        private void InitItemChart(int num)
        {
            itemCharts[num].onPointUpCallback = OnPointUpItemChart;
            itemCharts[num].Init();
            itemCharts[num].SetColor(colors[num]);
        }

        public override void Show()
        {
            Refresh();
            base.Show();
        }

        private void SetTitle(string str = "")
        {
            txtTitle.text = str;
        }

        public void Refresh()
        {
            (string Category, int Value)[] datas = cDataManager.Instance.GetChartData().ToArray();
            if (datas.Any())
            {
                SetTitle(GetText("TabChartTitle_All"));
                float total = datas.Sum(x => x.Value);
                float max = datas.Max(x => x.Value);
                for (int i = 0; i < itemCharts.Length; ++i)
                {
                    if (i < datas.Length)
                    {
                        itemCharts[i].SetTxt(datas[i].Category);
                        itemCharts[i].SetImg(datas[i].Value / max);
                        itemCharts[i].SetRatio($"{datas[i].Value / total * 100f:F0}%");
                        itemCharts[i].SetValue($"{datas[i].Value:N0}");
                        itemCharts[i].SetColor(colors[i]);
                        itemCharts[i].Show();
                    }
                    else
                    {
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
            SetTitle();
            rtNoData.gameObject.SetActive(true);
        }
        private void HideNodata()
        {
            rtNoData.gameObject.SetActive(false);
        }

        private void RefreshChart(string category, Color32 color)
        {// 카테고리별 월별 소비 변화 차트로 표시
            (int year, int month, int value)[] datas = cDataManager.Instance.GetChartData(category).ToArray();
            if (datas.Any())
            {
                SetTitle(category);
                float total = datas.Sum(x => x.value);
                float max = datas.Max(x => x.value);
                for (int i = 0; i < itemCharts.Length; ++i)
                {
                    if (i < datas.Length)
                    {
                        itemCharts[i].SetTxt($"{datas[i].year:0000}.{datas[i].month:00}");
                        itemCharts[i].SetImg(datas[i].value / max);
                        if (i > 0)
                        {
                            float ratio = (datas[i].value - datas[i - 1].value) * 100f / datas[i - 1].value;
                            itemCharts[i].SetRatio($"{((ratio > 0) ? "+" : "")}{ratio:F0}%");
                        }
                        else
                        {
                            itemCharts[i].SetRatio("");
                        }
                        itemCharts[i].SetValue($"{datas[i].value:N0}");
                        itemCharts[i].SetColor(color);
                        itemCharts[i].Show();
                    }
                    else
                    {
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

        private void OnPointUpItemChart(string category, Color32 color)
        {
            switch(currChartType)
            {
                case ChartType.All:
                    currChartType = ChartType.TargetCategory;
                    RefreshChart(category, color);
                    break;

                case ChartType.TargetCategory:
                    currChartType = ChartType.All;
                    Refresh();
                    break;

                default:
                    LogError($"Error : TabChart : itemNum {category} ChartType = {currChartType}");
                    break;
            }
        }
    }
}
