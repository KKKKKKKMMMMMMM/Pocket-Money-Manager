using KMUtils.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Data
{
    public class ItemDFList : MonoBehaviour
    {
        [SerializeField] private Text[] txts;
        private Button btn;
        private cDataField data;
        public Action<cDataField> onClickCallback;
        private bool isInit = false;

        private void Awake()
        {
            InitBtn();
        }

        private void InitBtn()
        {
            if (isInit) return; isInit = true;
            btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClickBtn);
        }
        private void OnClickBtn()
        {
            onClickCallback?.Invoke(data);
        }

        public void Reset()
        {
            foreach(Text txt in txts)
            {
                txt.text = "-";
            }
        }

        private string GetText(string key)
        {
            return cDataManager.Instance.GetText(key);
        }

        public void Show(cDataField _data)
        {
            data = _data;
            Refresh();
            gameObject.SetActive(true);
        }

        private void Refresh()
        {
            string[] strdata = new string[5];
            string dow = GetText($"DayOfWeek{(int)data.date.DayOfWeek}");
            strdata[0] = $"{data.date.Year:0000}-{data.date.Month:00}-{data.date.Day:00} ({dow})";
            strdata[1] = $"{data.category}";
            if (data.type == MoneyType.In)
            {
                strdata[2] = $"{GetText("InputMoney")} <color=blue>{data.value}</color>{GetText("MoneyType")}";
            }
            else
            {
                strdata[2] = $"{GetText("OutputMoney")} <color=red>{data.value}</color>{GetText("MoneyType")}";
            }
            strdata[3] = "";//$"{GetText("RemainingMoney")} {0}{GetText("MoneyType")}";
            strdata[4] = $"{data.info}";

            for (int i = 0; i < txts.Length; ++i)
            {
                txts[i].text = strdata[i];
            }
        }

        public void Show(string[] data)
        {
            Set(data);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            data = null;
            Reset();
            gameObject.SetActive(false);
        }

        private void Set(string[] data)
        {
            for (int i = 0; i < txts.Length; ++i)
            {
                txts[i].text = data[i];
            }
        }
    }
}
