using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMManager
{
    public class cLoadingManager : Singleton<cLoadingManager>
    {
        [SerializeField] private GameObject panelLoading;

        public static void ShowLoading()
        {
            Instance.panelLoading.SetActive(true);
        }

        public static void HideLoading()
        {
            Instance.panelLoading.SetActive(false);
        }
    }
}