using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel.Chart
{
    public class ItemChart : MonoBehaviour
    {
        [SerializeField] private RectTransform rtImg;
        [SerializeField] private Text txt;
        private Image img;
        [SerializeField] private float initSizeY;

        private void Awake()
        {
            Init();
        }

        private bool isInit = false;
        public void Init()
        {
            if(isInit)
            {
                return;
            }
            isInit = true;

            img = rtImg.GetComponent<Image>();
            initSizeY = rtImg.sizeDelta.y;
        }

        public void SetColor(Color32 color)
        {
            img.color = color;
        }

        public void SetImg(float value)
        {
            rtImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initSizeY * value);
        }

        public void SetTxt(string str)
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