using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel
{
    public class PanelHelp : PanelBase
    {
        [SerializeField] private RectTransform rtHighlight;
        [SerializeField] private RectTransform rtTxtBox;
        private Text txt;

        private Queue<HelpData> queue = new Queue<HelpData>();
        private HelpData currQue;

        public override void Init()
        {
            if (isInit) return;
            isInit = true;
            Log("Init : PanelHelp");
            txt = rtTxtBox.GetComponentInChildren<Text>();
            SetText();
        }

        private void SetText(string str = "")
        {
            if (str != "")
            {
                txt.text = str;
            }
            rtTxtBox.gameObject.SetActive(str != "");
        }

        public void Refresh(RectTransform target, string str = "")
        {
            Log($"RefreshHelp\nTarget : {target.name}\nMessage : {str}");
            SetText(str);
            SetHighlight(rtHighlight, target);
            Show();
        }

        private void SetHighlight(RectTransform _main, RectTransform _target)
        {
            // 크기를 동일하게 맞춤
            _main.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _target.rect.width);
            _main.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _target.rect.height);
            _main.pivot = _target.pivot;
            _main.position = _target.position;
            _main.gameObject.SetActive(true);
        }

        public void OnClickHelp()
        {
            Log("OnClickHelp");
            currQue.callback?.Invoke();
            if (queue.TryDequeue(out currQue))
            {
                Refresh(currQue.target, currQue.message);
            }
            else
            {
                Hide();
            }
        }
        public void AddQue(RectTransform target, string message, Action callback = null)
        {
            queue.Enqueue(new HelpData(target, message, callback));
        }
        public void AddQue(HelpData data)
        {
            queue.Enqueue(data);
        }
        public void StartQue()
        {
            if (queue.TryDequeue(out currQue))
            {
                Refresh(currQue.target, currQue.message);
            }
            else
            {
                Hide();
            }
        }
    }
}