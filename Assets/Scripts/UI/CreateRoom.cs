using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    public ToggleGroup chiToggleGroup;
    public ToggleGroup gameCycleToggleGroup;
    public Text diamondCostNumber;
    public Button generalModeCreateBtn;

    private void Awake()
    {
        generalModeCreateBtn.onClick.AddListener(CreateGeneralModeRoom);
        // 给所有圈数开关挂上请求钻石消耗回调。 
        foreach (Transform toggle in gameCycleToggleGroup.transform)
        {
            toggle.GetComponent<Toggle>().onValueChanged.AddListener(OnGameCycleChange);
        }
    }

    private void Start()
    {
        // 初始化钻石开销
        UpdateDiamondCostNumber();
    }

    // 更改游戏圈数后，更新钻石开销
    public void OnGameCycleChange(bool isOn)
    {
        if (isOn)
        {
            UpdateDiamondCostNumber();
        }
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

    // 更新钻石开销文本
    public void UpdateDiamondCostNumber()
    {
        int gameCycleNumber = GetGameCycleNumber();
        diamondCostNumber.text = gameCycleNumber.ToString();
    }
}