using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Data
{
    [RequireComponent(typeof(Toggle))]
    public class ItemTgl : MonoBehaviour
    {
        private Toggle tgl;
        [SerializeField] private Text txt;

        public Action<bool> onValueChanged;

        public bool IsOn
        {
            get
            {
                return tgl.isOn;
            }
            set
            {
                tgl.isOn = true;
            }
        }

        public void Init(string str = "")
        {
            if (tgl == null)
            {
                tgl = GetComponent<Toggle>();
                tgl.onValueChanged.AddListener(OnValueChanged);
            }
            SetText(str);
            Show();
        }

        private void OnValueChanged(bool isOn)
        {
            onValueChanged?.Invoke(isOn);
        }

        public string GetText()
        {
            return txt.text;
        }

        public void SetText(string str)
        {
            txt.text = str;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}