using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    // 充值金币，充值钻石，日常奖励
    public Button rechargeCoin, rechargeDiamond, dailyRewards;
    public Button generalMode;
    public Button createRoom, joinRoom;
    public Button task, sameCity, mall, events, setting;
    public Text coinNumber, diamondNumber;

    // 更新金币数量
    public void UpdateCoinNumber(int number)
    {
        coinNumber.text = number.ToString();
    }

    // 更新钻石数量
    public void UpdateDiamondNumber(int number)
    {
        diamondNumber.text = number.ToString();
    }

    // 登录后更新用户信息
    public void UpdateProfile(int coinNumber, int diamondNumber)
    {
        UpdateCoinNumber(coinNumber);
        UpdateDiamondNumber(diamondNumber);
    }
}