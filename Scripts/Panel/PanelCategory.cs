//#define LogPanelCategory

using KMUtils.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Manager;

namespace KMUtils.Panel
{
    public class PanelCategory : PanelBase
    {
        [SerializeField] private Text txtTitle;
        [SerializeField] private Button btnQuit;
        [SerializeField] private Button btnAdd;
        [SerializeField] private ItemCategory[] items;
        private List<string> data = new List<string>();
        private bool isChange = false;

        private void Awake()
        {
            if (isInit == false)
            {
                Init();
            }
        }

        public override void Init()
        {
            if(isInit)
            {
                return;
            }
            isInit = true;

            txtTitle.text = GetText("TitlePanelCategory");
            btnQuit.onClick.AddListener(OnClickQuit);           
            btnAdd.onClick.AddListener(OnClickAdd);

            for (int i = 0; i < items.Length; ++i)
            {
                int num = i;
                items[num].onValueChangeField = (str) =>
                {
                    isChange = true;
                    OnValueChangeData(num, str);
                };
                items[num].onClickDelete = delegate
                {
                    isChange = true;
                    OnClickDelete(num);
                };
            }
        }

        public override void Show()
        {
            Load();
            base.Show();
        }

        public override void Hide()
        {
            if (isChange)
            {
                Save();
            }
            base.Hide();
        }

        private void Load()
        {
            data.Clear();
            data.AddRange(cDataManager.Instance.GetCategorys());
            Log($"Load Category {data.Count}");
            Refresh();
        }

        private void Save()
        {
            string[] data = items.Select(x => x.GetField()).Where(x => string.IsNullOrWhiteSpace(x)).ToArray();
            iMain.SetCategory(data);
        }

        private void Refresh()
        {
            for (int i = 0; i < items.Length; ++i)
            {
                items[i].Hide();
            }
            for (int i = 0; i < data.Count; ++i)
            {
                items[i].SetField(data[i]);
                items[i].Show();
            }
            RefreshBtnAdd();
        }

        private void RefreshBtnAdd()
        {
            bool isActive = items.Where(x => x.gameObject.activeSelf == false).Count() > 0;
            btnAdd.gameObject.SetActive(isActive);
        }

        private void OnClickQuit()
        {
            Hide();
        }
        private void OnClickAdd()
        {
            Log($"OnClickAdd {data.Count}/{items.Length}");
            if (data.Count < items.Length)
            {
                data.Add("");
            }
            Refresh();
        }

        private void OnValueChangeData(int idx, string str)
        {
            Log($"OnValueChangeData : data[{idx}] = {str}");
            data[idx] = str;
        }

        private void OnClickDelete(int idx)
        {
            Log($"OnClickDelete {idx}");
            data.RemoveAt(idx);
            Refresh();
        }
    }
}