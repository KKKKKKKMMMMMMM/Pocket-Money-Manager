using KMUtils.Manager;
using UnityEngine;


namespace KMUtils.Panel
{
    public abstract class PanelBase : MonoBehaviour
    {
        protected InterMain iMain;
        public InterMain IMain
        {
            get
            {
                return iMain;
            }
            set
            {
                iMain = value;
            }
        }

        protected InterPanel iPanel;
        public InterPanel IPanel
        {
            get
            {
                return iPanel;
            }
            set
            {
                iPanel = value;
            }
        }

        protected bool isInit = false;
        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }

        private void Awake()
        {
            Init();
        }

        public abstract void Init();
        public virtual void Show()
        {
            Log($"Panel Show : {gameObject.name}");
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            Log($"Panel Hide : {gameObject.name}");
            gameObject.SetActive(false);
        }

        [SerializeField] protected bool isHideLog = false;
        public virtual void Log(string str)
        {
            if (isHideLog) return;
            LogManager.Log(str);
        }
        public virtual void LogWarning(string str)
        {
            if (isHideLog) return;
            LogManager.LogWarning(str);
        }
        public virtual void LogError(string str)
        {
            if (isHideLog) return;
            LogManager.LogError(str);
        }

        protected string GetText(string key)
        {
            return cDataManager.Instance.GetText(key);
        }
    }
}