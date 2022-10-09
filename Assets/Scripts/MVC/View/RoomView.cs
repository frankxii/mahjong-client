using System;
using System.Collections.Generic;
using Data;
using MVC.Base;
using MVC.Model;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public enum SeatPos
    {
        Self,
        Opposite,
        Left,
        Right
    }

    public class RoomView : BaseView
    {
        public Text txtRemainCard; // 剩余牌数
        public Text txtRoomId; // 房间ID
        public Text txtCycle; // 当前圈数
        public Image imgDealerWind; // 中控
        public Button btnLeaveRoom; // 离开房间按钮
        public bool canPlayCard;

        public event Action<byte> onPlayCard; // 出牌事件
        public event Action<string> onOperation; // 操作事件 碰、杠、胡、过 

        private Dictionary<byte, Sprite> _selfHandCardMapping = new(); // 本家手牌图片字典
        private Dictionary<byte, Sprite> _selfPlayCardMapping = new(); // 本家出牌图片字典
        private Dictionary<byte, Sprite> _leftPlayCardMapping = new(); // 上家出牌图片字典
        private Dictionary<byte, Sprite> _rightPlayCardMapping = new(); // 下家出牌图片字典
        private Dictionary<string, Sprite> _operationButtonMapping = new(); // 操作按钮图片字典

        private GameObject _selfHandCardPrefab; // 本家手牌prefab
        private GameObject _oppositeHandCardPrefab; // 对家手牌prefab
        private GameObject _leftHandCardPrefab; // 上家手牌prefab
        private GameObject _rightHandCardPrefab; // 下家手牌prefab
        private GameObject _selfPlayCardPrefab; // 本家出牌prefab
        private GameObject _leftPlayCardPrefab; // 上家出牌prefab
        private GameObject _rightPlayCardPrefab; // 下家出牌prefab
        private GameObject _operationButtonPrefab; // 操作按钮prefab

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

        [Header("出牌区域")]
        public Transform selfPlayCardPos1;
        public Transform selfPlayCardPos2;
        public Transform oppositePlayCardPos1;
        public Transform oppositePlayCardPos2;
        public Transform leftPlayCardPos1;
        public Transform leftPlayCardPos2;
        public Transform rightPlayCardPos1;
        public Transform rightPlayCardPos2;

        [Header("本家")]
        public Image imgSelfAvatar; // 头像
        public Text txtSelfUsername; // 用户名
        public Text txtSelfCoinNumber; // 金币
        public Transform selfHandCardPos; // 手牌初始挂载位置
        public Transform selfDrawCardPos; // 摸牌挂载位置
        public Transform operationArea; // 操作按钮挂载位置

        [Header("对家")]
        public Image imgOppositeAvatar;
        public Text txtOppositeUsername;
        public Text txtOppositeCoinNumber;
        public Transform oppositeHandCardPos;
        public Transform oppositeDrawCardPos;

        [Header("上家")]
        public Image imgLeftAvatar;
        public Text txtLeftUsername;
        public Text txtLeftCoinNumber;
        public Transform leftHandCardPos;
        public Transform leftDrawCardPos;

        [Header("下家")]
        public Image imgRightAvatar;
        public Text txtRightUsername;
        public Text txtRightCoinNumber;
        public Transform rightHandCardPos;
        public Transform rightDrawCardPos;


        private void Start()
        {
            Dictionary<string, Dictionary<byte, Sprite>> imgMapping = new()
            {
                ["Art/SelfHandCard"] = _selfHandCardMapping,
                ["Art/SelfPlayCard"] = _selfPlayCardMapping,
                ["Art/LeftPlayCard"] = _leftPlayCardMapping,
                ["Art/RightPlayCard"] = _rightPlayCardMapping
            };
            // 加载手牌和出牌图片
            foreach (string path in imgMapping.Keys)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(path);
                Dictionary<byte, Sprite> dict = imgMapping[path];
                foreach (Sprite sprite in sprites)
                {
                    dict.Add(byte.Parse(sprite.name), sprite);
                }
            }

            // 加载操作按钮图片
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("Art/OperationButton"))
            {
                _operationButtonMapping.Add(sprite.name, sprite);
            }

            // 加载手牌prefab
            _selfHandCardPrefab = Resources.Load<GameObject>("Card/SelfHandCardPrefab");
            _oppositeHandCardPrefab = Resources.Load<GameObject>("Card/OppositeHandCardPrefab");
            _leftHandCardPrefab = Resources.Load<GameObject>("Card/LeftHandCardPrefab");
            _rightHandCardPrefab = Resources.Load<GameObject>("Card/RightHandCardPrefab");
            // 加载出牌prefab
            _selfPlayCardPrefab = Resources.Load<GameObject>("Card/SelfPlayCardPrefab");
            _leftPlayCardPrefab = Resources.Load<GameObject>("Card/LeftPlayCardPrefab");
            _rightPlayCardPrefab = Resources.Load<GameObject>("Card/RightPlayCardPrefab");
            // 加载操作按钮prefab
            _operationButtonPrefab = Resources.Load<GameObject>("UI/OperationButtonPrefab");
        }

        // 通过本家门风和玩家门风，来判断玩家位置是本家、对家、上家、下家
        private SeatPos DealerWindToSeatPos(byte selfDealerWind, byte playerDealerWind)
        {
            int value = playerDealerWind - selfDealerWind;

            if (value == 2 || value == -2)
                return SeatPos.Opposite;
            else if (value == 1 || value == -3)
                return SeatPos.Right;
            else if (value == -1 || value == 3)
                return SeatPos.Left;
            else
                return SeatPos.Self;
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
                SeatPos seat = DealerWindToSeatPos(dealerWind, player.dealerWind);
                switch (seat)
                {
                    case SeatPos.Self:
                        txtSelfUsername.text = player.username;
                        txtSelfCoinNumber.text = player.coin.ToString();
                        imgSelfAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                        imgSelfReady.gameObject.SetActive(player.isReady);
                        break;
                    case SeatPos.Opposite:
                        txtOppositeUsername.text = player.username;
                        txtOppositeCoinNumber.text = player.coin.ToString();
                        imgOppositeAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                        imgOppositeReady.gameObject.SetActive(player.isReady);
                        break;
                    case SeatPos.Left:
                        txtLeftUsername.text = player.username;
                        txtLeftCoinNumber.text = player.coin.ToString();
                        imgLeftAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                        imgLeftReady.gameObject.SetActive(player.isReady);
                        break;
                    case SeatPos.Right:
                        txtRightUsername.text = player.username;
                        txtRightCoinNumber.text = player.coin.ToString();
                        imgRightAvatar.sprite = player.gender == 1 ? boyAvatar : girlAvatar;
                        imgRightReady.gameObject.SetActive(player.isReady);
                        break;
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
                HandCard component = cardObject.GetComponent<HandCard>();
                component.card = handCards[index];
                component.onPlayCard += OnPlayCard;
                // 处理出牌逻辑，广播出牌事件，controller接听出牌事件
                offset += 115;
            }
        }

        // 本家出牌回调
        private void OnPlayCard(byte card, GameObject go)
        {
            // 当前不能出牌
            if (!canPlayCard)
                return;

            // 出的手牌区的牌
            if (go.transform.parent.name == "selfHandCardPos")
            {
                Vector3 pos = go.transform.localPosition;
                // 有一张牌出掉，前面的牌需要补位
                foreach (Transform cardObject in selfHandCardPos)
                {
                    if (cardObject.localPosition.x < pos.x)
                        cardObject.localPosition += new Vector3(115, 0);
                }

                if (selfDrawCardPos.childCount > 0)
                {
                    int offset = 0;
                    int index = 0;
                    Transform drawCardObject = selfDrawCardPos.GetChild(0);
                    byte cardNumber = drawCardObject.gameObject.GetComponent<HandCard>().card;
                    foreach (Transform cardObject in selfHandCardPos)
                    {
                        if (cardObject.gameObject.GetComponent<HandCard>().card > cardNumber)
                        {
                            if (cardObject.gameObject == go)
                                continue;
                            offset -= 115;
                            index += 1;
                        }
                        else
                        {
                            cardObject.localPosition -= new Vector3(115, 0);
                        }
                    }

                    // 设置新摸的牌的位置以及层级顺序
                    drawCardObject.SetParent(selfHandCardPos);
                    drawCardObject.localPosition = new Vector3(offset, 0);
                    drawCardObject.SetSiblingIndex(index);
                }
            }

            canPlayCard = false; // 出完一张牌后不能再继续出牌
            Destroy(go);
            // 在出牌区增加一张牌
            Transform parent = selfPlayCardPos1.childCount < 12 ? selfPlayCardPos1 : selfPlayCardPos2;
            GameObject playCard = Instantiate(_selfPlayCardPrefab, parent);
            playCard.GetComponent<Image>().sprite = _selfPlayCardMapping[card];
            playCard.transform.localPosition = new Vector3(parent.childCount * 70, 0);

            // 回调controller
            onPlayCard?.Invoke(card);
        }

        /// <summary>
        /// 展示可操作按钮
        /// </summary>
        /// <param name="operationList">可操作按钮字符串列表</param>
        private void ShowOperationButton(List<string> operationList)
        {
            for (int index = 0; index < operationList.Count; index++)
            {
                string operation = operationList[index];
                GameObject peng = Instantiate(_operationButtonPrefab, operationArea);
                peng.name = operation;
                peng.GetComponent<Image>().sprite = _operationButtonMapping[operation];
                peng.transform.localPosition = new Vector3(200 * index, 0);
                peng.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // 移除所有操作按钮
                    foreach (Transform child in operationArea)
                    {
                        Destroy(child.gameObject);
                    }

                    // 发布操作事件，通知controller
                    onOperation?.Invoke(operation);
                });
            }
        }

        public void OnOtherPlayCard(byte dealerWind, PlayCardEvent data)
        {
            SeatPos seat = DealerWindToSeatPos(dealerWind, data.dealerWind);
            // 判断出牌方位

            if (seat == SeatPos.Opposite)
            {
                // 在出牌区添加一张牌
                Transform parent = oppositePlayCardPos1.childCount < 12
                    ? oppositePlayCardPos1
                    : oppositePlayCardPos2;
                // 对家和本家是共用的出牌prefab
                GameObject playCard = Instantiate(_selfPlayCardPrefab, parent);
                playCard.GetComponent<Image>().sprite = _selfPlayCardMapping[data.card];
                playCard.transform.localPosition = new Vector3(parent.childCount * 70, 0);
            }
            else if (seat == SeatPos.Left)
            {
                Transform parent = leftPlayCardPos1.childCount < 12 ? leftPlayCardPos1 : leftPlayCardPos2;
                GameObject playCard = Instantiate(_leftPlayCardPrefab, parent);
                playCard.GetComponent<Image>().sprite = _leftPlayCardMapping[data.card];
                playCard.transform.localPosition = new Vector3(0, parent.childCount * -50);
            }
            else if (seat == SeatPos.Right)
            {
                Transform parent = rightPlayCardPos1.childCount < 12 ? rightPlayCardPos1 : rightPlayCardPos2;
                GameObject playCard = Instantiate(_rightPlayCardPrefab, parent);
                playCard.GetComponent<Image>().sprite = _rightPlayCardMapping[data.card];
                playCard.transform.localPosition = new Vector3(0, parent.childCount * 50);
                // 下家出牌，先出的牌需要放在最下面以覆盖后出牌的layer，所以每次实例化时把object的索引设为0
                parent.transform.SetSiblingIndex(0);
            }

            // 根据可以操作的情况，生成操作列表，展示对应操作按钮
            List<string> operationList = new();
            if (data.canPeng)
                operationList.Add("peng");

            if (data.canGang)
                operationList.Add("gang");

            if (data.canHu)
                operationList.Add("hu");

            operationList.Add("pass");
            ShowOperationButton(operationList);
        }

        public void OnDealCard(List<byte> handCards)
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

        public void OnDrawCard(byte dealerWind, DrawCardEvent data)
        {
            // 判断摸牌玩家位置
            SeatPos seat = DealerWindToSeatPos(dealerWind, data.dealerWind);
            switch (seat)
            {
                case SeatPos.Self:
                    GameObject cardObject = Instantiate(_selfHandCardPrefab, selfDrawCardPos);
                    cardObject.GetComponent<Image>().sprite = _selfHandCardMapping[data.card];
                    HandCard component = cardObject.GetComponent<HandCard>();
                    component.card = data.card;
                    component.onPlayCard += OnPlayCard;
                    canPlayCard = true;
                    break;
                case SeatPos.Opposite:
                    Instantiate(_oppositeHandCardPrefab, oppositeDrawCardPos);
                    break;
                case SeatPos.Left:
                    Instantiate(_leftHandCardPrefab, leftDrawCardPos);
                    break;
                case SeatPos.Right:
                    Instantiate(_rightHandCardPrefab, rightDrawCardPos);
                    break;
            }
        }
    }
}