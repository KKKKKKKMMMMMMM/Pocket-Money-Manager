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
        public Action onClickDelete;

        public void Init()
        {
            btnDelete.onClick.AddListener(OnClickDelete);
            Hide(true);
        }

        private void OnClickDelete()
        {
            Hide(true);
            onClickDelete?.Invoke();
        }

        public void Show(string str = "")
        {
            SetField(str);
            gameObject.SetActive(true);
        }

        public void Hide(bool isReset = false)
        {
            if (isReset)
            {
                SetField();
            }
            gameObject.SetActive(false);
        }

        private void SetField(string str = "")
        {
            field.text = str;
        }

        public string GetField()
        {
            return field.text;
        }
    }
}
