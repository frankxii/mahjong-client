using System.Collections.Generic;
using Data;
using MVC.Base;
using MVC.Model;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class RoomView : BaseView
    {
        public Text txtRemainCard; // 剩余牌数
        public Text txtRoomId; // 房间ID
        public Text txtCycle; // 当前圈数
        public Image imgDealerWind; // 中控
        public Button btnLeaveRoom; // 离开房间按钮

        private Dictionary<byte, Sprite> _selfHandCardMapping = new(); // 本家手牌图片字典
        private GameObject _selfHandCardPrefab; // 本家手牌prefab
        private GameObject _oppositeHandCardPrefab; // 对家手牌prefab
        private GameObject _leftHandCardPrefab; // 上家手牌prefab
        private GameObject _rightHandCardPrefab; // 下家手牌prefab

        [Header("东南西北图片")]
        public Sprite eastWind;
        public Sprite southWind;
        public Sprite westWind;
        public Sprite northWind;

        [Header("男女头像图片")]
        public Sprite boyAvatar;
        public Sprite girlAvatar;
        public Sprite defaultAvatar;

        [Header("玩家准备")]
        public Button btnReady;
        public Image imgSelfReady;
        public Image imgOppositeReady;
        public Image imgLeftReady;
        public Image imgRightReady;

        [Header("本家")]
        public Image imgSelfAvatar; // 头像
        public Text txtSelfUsername; // 用户名
        public Text txtSelfCoinNumber; // 金币
        public Transform selfHandCardPos; // 手牌初始挂载位置

        [Header("对家")]
        public Image imgOppositeAvatar;
        public Text txtOppositeUsername;
        public Text txtOppositeCoinNumber;
        public Transform oppositeHandCardPos;

        [Header("上家")]
        public Image imgLeftAvatar;
        public Text txtLeftUsername;
        public Text txtLeftCoinNumber;
        public Transform leftHandCardPos;

        [Header("下家")]
        public Image imgRightAvatar;
        public Text txtRightUsername;
        public Text txtRightCoinNumber;
        public Transform rightHandCardPos;


        private void Start()
        {
            // 加载手牌图片
            Sprite[] sprites = Resources.LoadAll<Sprite>("Art/SelfHandCard");
            foreach (Sprite sprite in sprites)
            {
                _selfHandCardMapping.Add(byte.Parse(sprite.name), sprite);
            }

            // 加载手牌prefab
            _selfHandCardPrefab = Resources.Load<GameObject>("Card/SelfHandCardPrefab");
            _oppositeHandCardPrefab = Resources.Load<GameObject>("Card/OppositeHandCardPrefab");
            _leftHandCardPrefab = Resources.Load<GameObject>("Card/LeftHandCardPrefab");
            _rightHandCardPrefab = Resources.Load<GameObject>("Card/RightHandCardPrefab");
        }

        /// <summary>
        /// 更新房间基础信息
        /// </summary>
        /// <param name="model"></param>
        public void UpdateRoomInfo(RoomModel model)
        {
            txtRoomId.text = model.RoomId.ToString();
            txtCycle.text = model.CurrentCycle.ToString() + "/" + model.TotalCycle.ToString();
        }

        private void ResetPlayersInfo()
        {
            txtOppositeUsername.text = "";
            txtOppositeCoinNumber.text = "";
            imgOppositeAvatar.sprite = defaultAvatar;
            imgOppositeReady.gameObject.SetActive(false);

            txtLeftUsername.text = "";
            txtLeftCoinNumber.text = "";
            imgLeftAvatar.sprite = defaultAvatar;
            imgLeftReady.gameObject.SetActive(false);

            txtRightUsername.text = "";
            txtRightCoinNumber.text = "";
            imgRightAvatar.sprite = defaultAvatar;
            imgRightReady.gameObject.SetActive(false);
        }

        // 准备按钮回调
        public void OnReady()
        {
            imgSelfReady.gameObject.SetActive(true);
            btnReady.gameObject.SetActive(false);
        }

        // 关闭所有准备状态的勾号
        public void HideAllReadyFlag()
        {
            imgSelfReady.gameObject.SetActive(false);
            imgOppositeReady.gameObject.SetActive(false);
            imgLeftReady.gameObject.SetActive(false);
            imgRightReady.gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新玩家座位与昵称、金币等信息
        /// </summary>
        /// <param name="dealerWind"></param>
        /// <param name="players"></param>
        public void UpdatePlayerInfo(byte dealerWind, List<PlayerInfo> players)
        {
            // 重置玩家信息，方便更新玩家信息时清除已离开玩家的信息
            ResetPlayersInfo();

            // 设置门风
            imgDealerWind.sprite = dealerWind switch
            {
                1 => eastWind,
                2 => southWind,
                3 => westWind,
                _ => northWind
            };
            foreach (PlayerInfo player in players)
            {
                // 东南西北门风的值为1234，通过玩家门风与本家的差值，来判断每个玩家所在的位置
                int value = player.dealerWind - dealerWind;
                if (value == 0)
                {
                    txtSelfUsername.text = player.username;
                    txtSelfCoinNumber.text = player.coin.ToString();
                    imgSelfAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                    imgSelfReady.gameObject.SetActive(player.isReady);
                }
                else if (value == 2 || value == -2)
                {
                    txtOppositeUsername.text = player.username;
                    txtOppositeCoinNumber.text = player.coin.ToString();
                    imgOppositeAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                    imgOppositeReady.gameObject.SetActive(player.isReady);
                }
                else if (value == 1 || value == -3)
                {
                    txtRightUsername.text = player.username;
                    txtRightCoinNumber.text = player.coin.ToString();
                    imgRightAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                    imgRightReady.gameObject.SetActive(player.isReady);
                }
                else if (value == -1 || value == 3)
                {
                    txtLeftUsername.text = player.username;
                    txtLeftCoinNumber.text = player.coin.ToString();
                    imgLeftAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                    imgLeftReady.gameObject.SetActive(player.isReady);
                }
            }
        }

        private void ShowHandCards(List<byte> handCards)
        {
            foreach (Transform child in selfHandCardPos)
            {
                Destroy(child.gameObject);
            }

            // 初始化本家手牌
            int offset = 0;

            // 倒序遍历
            for (int index = handCards.Count - 1; index > -1; index--)
            {
                GameObject cardObject = Instantiate(_selfHandCardPrefab, selfHandCardPos);
                cardObject.GetComponent<Image>().sprite = _selfHandCardMapping[handCards[index]];
                cardObject.transform.localPosition += offset * Vector3.left;
                offset += 115;
            }
        }

        public void DealCard(List<byte> handCards)
        {
            ShowHandCards(handCards);

            // 初始化对家手牌
            int offset = 0;
            for (int i = 0; i < 13; i++)
            {
                GameObject cardObject = Instantiate(_oppositeHandCardPrefab, oppositeHandCardPos);
                cardObject.transform.localPosition += offset * Vector3.right;
                offset += 70;
            }

            // 初始化上家手牌
            offset = 0;
            for (int i = 0; i < 13; i++)
            {
                GameObject cardObject = Instantiate(_leftHandCardPrefab, leftHandCardPos);
                cardObject.transform.localPosition += offset * Vector3.down;
                offset += 40;
            }

            // 初始化下家手牌
            offset = 0;
            for (int i = 0; i < 13; i++)
            {
                GameObject cardObject = Instantiate(_rightHandCardPrefab, rightHandCardPos);
                cardObject.transform.localPosition += offset * Vector3.down;
                offset += 40;
            }
        }

        public void SortCard(List<byte> handCards)
        {
            handCards.Sort();
            ShowHandCards(handCards);
        }
    }
}