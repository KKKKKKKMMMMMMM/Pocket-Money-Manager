using KMManager;
using KMUtils.Data;
using KMUtils.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMUtils
{
    public class Main : MonoBehaviour, InterMain
    {
        [SerializeField] private cPanelManager panelManager;

        [SerializeField] private cDataManager dataManager;
        public cDataManager DataManager
        {
            get
            {
                return dataManager;
            }
        }


        private void Awake()
        {

        }

        private void Start()
        {
            InitSetQuit();
            StartCoroutine(WaitInit());
        }

        private void Update()
        {

        }

        private IEnumerator WaitInit()
        {
            cLoadingManager.ShowLoading();
            float time = Time.realtimeSinceStartup;
            yield return new WaitUntil(() => dataManager.IsInit);

            panelManager.Init(this);

#if UNITY_EDITOR
            yield return new WaitUntil(() => Time.realtimeSinceStartup - time > 3f);
#endif
            cLoadingManager.HideLoading();
        }

        private void InitSetQuit()
        {
            Application.wantsToQuit += () =>
            {
                return true;
            };
        }

        public string GetSortText()
        {
            return dataManager.GetSortText();
        }

        public string GetText(string key)
        {
            return dataManager.GetText(key);
        }

        public string[] GetCategorys()
        {
            return dataManager.GetCategorys();
        }
        public string GetCategory(int idx)
        {
            return dataManager.GetCategorys()[idx];
        }

        public void SetCategory(string[] data)
        {
            dataManager.SetCategorys(data);
        }

        public IEnumerable<cDataField> GetData()
        {
            return dataManager.GetData();
        }

        public void AddData(DateTime date, string category, MoneyType type, int value, string info)
        {
            dataManager.AddData(date, category, type, value, info);
        }
        public void AddData(cDataField data)
        {
            dataManager.AddData(data);
        }

        public void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}