using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel
{
    public class PanelCalendar : PanelBase
    {
        [SerializeField] private Button btnQuit;

        public override void Init()
        {
            btnQuit.onClick.AddListener(iPanel.ShowPanelMain);
        }
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }
    }
}