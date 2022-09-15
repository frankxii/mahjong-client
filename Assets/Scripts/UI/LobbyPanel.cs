using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    public static LobbyPanel panel;

    [Header("顶部")]
    public Image imgAvatar;
    public Text txtCoinNumber;
    public Button btnAddCoin;
    public Text txtDiamondNumber;
    public Button btnAddDiamond;

    [Header("底部")]
    public Button btnTask;
    public Button btnSameCity;
    public Button btnMall;
    public Button btnEvent;
    public Button btnDailyRewards;

    [Header("创建房间")]
    public Button btnCreateRoom;
    public Button btnJoinRoom;

    private void Start()
    {
        btnCreateRoom.onClick.AddListener(() =>
        {
            CreateRoomPanel.ShowPanel();
        });
    }

    public static void ShowPanel()
    {
        if (panel is null)
        {
            // 获取Prefab
            GameObject panelPrefab = Resources.Load<GameObject>("UI/LobbyPanel");
            // 实例化panel
            GameObject loginPanel = Instantiate(panelPrefab);
            // 挂载到UI canvas
            loginPanel.transform.SetParent(GameObject.Find("UICanvas").transform, false);
            // 绑定实例
            panel = loginPanel.GetComponent<LobbyPanel>();
        }

        panel.gameObject.SetActive(true);
    }

    public static void HidePanel()
    {
        if (panel is not null)
            panel.gameObject.SetActive(false);
    }
}