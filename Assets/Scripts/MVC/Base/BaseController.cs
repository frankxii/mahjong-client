using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log(ViewName);
            GameObject panelPrefab = Resources.Load<GameObject>(ViewName);
            // 实例化panel
            GameObject panel = GameObject.Instantiate(panelPrefab);
            // 挂载到UI canvas
            panel.transform.SetParent(GameObject.Find("UICanvas").transform, false);
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
}