using System;

namespace KMUtils.Panel
{
    public interface InterPanel
    {
        public void ShowPanelMain();
        public void ShowPanelList();
        public void ShowPopup(string msg, Action callbackok = null, Action callbackcancel = null);
    }
}