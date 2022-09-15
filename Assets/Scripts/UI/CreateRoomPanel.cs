using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{
    public static CreateRoomPanel panel;

    public Button btnGeneralMode;

    [Header("通用模式")]
    public ToggleGroup chiToggleGroup;
    public ToggleGroup gameCycleToggleGroup;
    public Text txtDiamondCostNumber;
    public Button btnGeneralModeCreate;

    public static void ShowPanel()
    {
        if (panel is null)
        {
            // 获取Prefab
            GameObject panelPrefab = Resources.Load<GameObject>("UI/CreateRoomPanel");
            // 实例化panel
            GameObject loginPanel = Instantiate(panelPrefab);
            // 挂载到UI canvas
            loginPanel.transform.SetParent(GameObject.Find("UICanvas").transform, false);
            // 绑定实例
            panel = loginPanel.GetComponent<CreateRoomPanel>();
        }

        panel.gameObject.SetActive(true);
    }

    public static void HidePanel()
    {
        if (panel is not null)
            panel.gameObject.SetActive(false);
    }

    private void Start()
    {
        btnGeneralMode.Select();
    }

    // 获取游戏圈数规则
    private int GetGameCycleNumber()
    {
        Toggle selectedToggle;
        // 获取游戏圈数
        selectedToggle = gameCycleToggleGroup.ActiveToggles().ToList()[0];
        int gameCycle = 4;
        if (selectedToggle.name == "SixteenCycleToggle")
            gameCycle = 16;
        else if (selectedToggle.name == "EightCycleToggle")
            gameCycle = 8;
        else if (selectedToggle.name == "FourCycleToggle")
            gameCycle = 4;
        return gameCycle;
    }

    // 创建房间
    private void CreateGeneralModeRoom()
    {
        Toggle selectedToggle = chiToggleGroup.ActiveToggles().ToList()[0];
        // 是否允许吃
        bool hasChi = selectedToggle.name == "CanChiToggle";
        int gameCycleNumber = GetGameCycleNumber();
    }
}