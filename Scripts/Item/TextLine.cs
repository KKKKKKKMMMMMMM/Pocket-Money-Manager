using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils
{
    public class TextLine : MonoBehaviour
    {
        [SerializeField] private Text txt;
        [SerializeField] private Image line;
        [SerializeField] private int offset = 20;

        public void SetLine(string str)
        {
            txt.text = str;
            float width = GetComponent<RectTransform>().rect.width;
            float size = width - (txt.preferredWidth + offset);
            if (size > 0)
            {
                line.fillAmount = size / width;
            }
            else
            {
                LogManager.LogError($"TextLine SizeError {str} / {txt.preferredWidth} / {offset} / {width}");
            }
        }
    }
}