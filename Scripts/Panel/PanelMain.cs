using UnityEngine;
using UnityEngine.UI;

namespace KMUtils.Panel
{
    public class PanelMain : PanelBase
    {
        [SerializeField] private Text txtTitle;
        [SerializeField] private Button[] btns;

        public override void Init()
        {
            InitText();
        }
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }

        private void InitText()
        {
            txtTitle.text = iMain.GetText("Main_Title");
            
            string[] key = new string[]
            {
                "Main_Btn1",
                "Main_Btn2",
                "Main_Btn3",
            };
            for (int i = 0; i < btns.Length; ++i)
            {
                SetBtnMain(i, iMain.GetText(key[i]));
            }
        }
        
        private void SetBtnMain(int num, string str)
        {
            Text txt = btns[num].GetComponentInChildren<Text>();
            if (txt != null)
            {
                txt.text = str;
            }
            btns[num].onClick.RemoveAllListeners();
            btns[num].onClick.AddListener(delegate { OnClickBtn(num); });
        }

        private void OnClickBtn(int num)
        {
            switch (num)
            {
                case 0:
                    iPanel.ShowPanelList();
                    break;
                case 1:
                    OnClickQuit();
                    //iPanel.ShowPanelCalendar();
                    break;
                case 2:
                    //iPanel.ShowPanelChart();
                    //iPanel.ShowPanelCategory();
                    break;
            }
        }

        private void OnClickQuit()
        {
            iMain.OnQuit();
        }
    }
}