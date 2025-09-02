using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMUtils
{
    public class HelpData
    {
        public RectTransform target;
        public string message;
        public Action callback;

        public HelpData(RectTransform _target, string _message, Action _callback = null)
        {
            target = _target;
            message = _message;
            callback = _callback;
        }
        public HelpData(GameObject _target, string _message, Action _callback = null)
        {
            target = _target.GetComponent<RectTransform>();
            message = _message;
            callback = _callback;
        }
    }
}