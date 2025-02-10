using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Data
{
    public class ItemCategory : MonoBehaviour
    {
        [SerializeField] private InputField field;
        [SerializeField] private Button btnDelete;
        public Action<string> onValueChangeField;
        public Action onClickDelete;

        private void Awake()
        {
            field.onValueChanged.AddListener(OnValueChangeField);
            btnDelete.onClick.AddListener(OnClickDelete);
        }

        private void OnValueChangeField(string str)
        {
            onValueChangeField?.Invoke(str);
        }

        private void OnClickDelete()
        {
            onClickDelete?.Invoke();
        }

        public void Show(bool isReset)
        {
            if (isReset)
            {
                SetField("");
            }
            gameObject.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetField(string str)
        {
            field.text = str;
        }

        public string GetField()
        {
            return field.text;
        }
    }
}
