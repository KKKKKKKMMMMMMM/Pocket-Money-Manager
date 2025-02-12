//#define LogPanelCategory

using KMUtils.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KMUtils.Manager;
using System;

namespace KMUtils.Panel
{
    public class PanelCategory : MonoBehaviour
    {
        [SerializeField] private Text txtTitle;
        [SerializeField] private Button btnQuit;
        [SerializeField] private Button btnAdd;
        [SerializeField] private ItemCategory[] items;

        private Action<string[]> hideCallback;
        private bool isInit = false;

        public void Show(string[] data)
        {
            Refresh(data);
            gameObject.SetActive(true);
        }

        public void Hide(bool isSave = true)
        {
            if (isSave)
            {
                string[] data = items.Select(x => x.GetField()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                hideCallback?.Invoke(data);
            }
            gameObject.SetActive(false);
        }

        public void Init(Action<string[]> callback)
        {
            if(isInit)
            {
                return;
            }
            isInit = true;

            txtTitle.text = cDataManager.Instance.GetText("TitlePanelCategory");
            btnQuit.onClick.AddListener(OnClickQuit);           
            btnAdd.onClick.AddListener(OnClickAdd);

            for (int i = 0; i < items.Length; ++i)
            {
                InitItem(i);
            }

            hideCallback = callback;

            Hide(false);
        }
        private void InitItem(int idx)
        {
            items[idx].onClickDelete = () => OnClickDelete(idx);
            items[idx].Init();
        }

        private void Refresh(string[] datas)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                items[i].Hide(true);
            }
            for (int i = 0; i < datas.Length; ++i)
            {
                items[i].Show(datas[i]);
            }
            RefreshBtnAdd();
        }

        private void RefreshBtnAdd()
        {
            btnAdd.gameObject.SetActive(items.Any(x => !x.gameObject.activeSelf));
        }

        private void OnClickQuit()
        {
            Hide();
        }
        private void OnClickAdd()
        {
            items.Where(x => x.gameObject.activeSelf == false).First().Show();
            RefreshBtnAdd();
        }
        private void OnClickDelete(int idx)
        {
            string[] datas = items.Where((x, index) => index != idx && x.gameObject.activeSelf).Select(x => x.GetField()).ToArray();
            Refresh(datas);
        }
    }
}