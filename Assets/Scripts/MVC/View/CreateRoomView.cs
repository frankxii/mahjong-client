using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomView : BaseView
{
    public Button btnGeneralMode;

    [Header("通用模式")]
    public ToggleGroup chiToggleGroup;
    public ToggleGroup gameCycleToggleGroup;
    public Text txtDiamondCostNumber;
    public Button btnGeneralModeCreate;


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