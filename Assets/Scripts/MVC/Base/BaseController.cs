using UnityEngine;

namespace MVC.Base
{
    public abstract class BaseController<TController, TView>
        where TController : BaseController<TController, TView>, new()
        where TView : MonoBehaviour
    {
        private static TController _instance; // 单例对象
        protected TView view; // 要挂载的视图脚本
        protected bool hasMounted; // UI是否已挂载
        protected abstract string ViewName { get; } //UI prefab路径

        // 单例对象属性
        public static TController Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new TController();
                }

                return _instance;
            }
        }

        protected abstract void OnViewMounted();


        public void ShowUI()
        {
            if (!hasMounted)
            {
                GameObject panelPrefab = Resources.Load<GameObject>(ViewName);
                // 实例化panel并挂载到UI Canvas
                GameObject panel = Object.Instantiate(panelPrefab, GameObject.Find("UICanvas").transform);
                view = panel.GetComponent<TView>();
                hasMounted = true;
                OnViewMounted();
            }

            view.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            view.gameObject.SetActive(false);
        }

        public virtual void Destroy()
        {
            Object.Destroy(view.gameObject);
            _instance = null;
        }
    }
}