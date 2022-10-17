using MVC.Base;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class ResultView : BaseView
    {
        public Text txtWinType;

        [Header("本家")]
        public Image imgSelfAvatar;
        public Text txtSelfScore;
        public Text txtSelfWinType;
        [Header("对家")]
        public Image imgOppositeAvatar;
        public Text txtOppositeScore;
        public Text txtOppositeWinType;
        [Header("上家")]
        public Image imgLeftAvatar;
        public Text txtLeftScore;
        public Text txtLeftWinType;
        [Header("下家")]
        public Image imgRightAvatar;
        public Text txtRightScore;
        public Text txtRightWinType;

        public Button btnContinue;
        public Button btnClose;
    }
}