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
            (string Category, int Value)[] datas = cDataManager.Instance.GetChartData().ToArray();
            if (datas.Any())
            {
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
            rtNoData.gameObject.SetActive(true);
        }
        private void HideNodata()
        {
            rtNoData.gameObject.SetActive(false);
        }

        public void OnClickCategory(int num)
        {// 카테고리별 월별 소비 변화 차트로 표시
            (int, int, int)[] datas = cDataManager.Instance.GetChartData(num).ToArray();
            if (datas.Any())
            {
                //float total = datas.Sum(x => x.Value);
                //float max = datas.Max(x => x.Value);
                //for (int i = 0; i < itemCharts.Length; ++i)
                //{
                //    if (i < datas.Length)
                //    {
                //        itemCharts[i].SetTxt(datas[i].Category);
                //        itemCharts[i].SetImg(datas[i].Value / max);
                //        itemCharts[i].SetRatio($"{datas[i].Value / total * 100f:F0}%");
                //        itemCharts[i].SetValue($"{datas[i].Value:N0}");
                //        itemCharts[i].Show();
                //    }
                //    else
                //    {
                //        itemCharts[i].Hide();
                //    }
                //}
                //HideNodata();
            }
            else
            {
                ShowNodata();
            }
        }

    }
}
