using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Data
{
    public class ItemDFList : MonoBehaviour
    {
        [SerializeField] private Text[] txts;

        private void Awake()
        {
            
        }

        public void Reset()
        {
            foreach(Text txt in txts)
            {
                txt.text = "-";
            }
        }

        public void Show(string[] data)
        {
            Set(data);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
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
