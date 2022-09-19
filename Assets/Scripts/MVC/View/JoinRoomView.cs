using MVC.Base;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View
{
    public class JoinRoomView : BaseView
    {
        private int _pointer = 1;

        [Header("房间号格子")]
        public Text txtInputField1;
        public Text txtInputField2;
        public Text txtInputField3;
        public Text txtInputField4;
        public Text txtInputField5;

        [Header("小键盘")]
        public Button btnNumber1;
        public Button btnNumber2;
        public Button btnNumber3;
        public Button btnNumber4;
        public Button btnNumber5;
        public Button btnNumber6;
        public Button btnNumber7;
        public Button btnNumber8;
        public Button btnNumber9;
        public Button btnNumber0;
        public Button btnClear;
        public Button btnDelete;

        [Header("加入房间，关闭窗口")]
        public Button btnJoinRoom;
        public Button btnClose;

        private Text GetInputField()
        {
            return _pointer switch
            {
                1 => txtInputField1,
                2 => txtInputField2,
                3 => txtInputField3,
                4 => txtInputField4,
                _ => txtInputField5
            };
        }

        // 响应小键盘数字输入
        public void ClickNumber(short number)
        {
            Text inputField = GetInputField();
            if (inputField.text == "")
            {
                inputField.text = number.ToString();
                _pointer = Mathf.Min(5, _pointer + 1);
            }
        }

        // 清空输入框
        public void Clear()
        {
            _pointer = 1;
            txtInputField1.text = "";
            txtInputField2.text = "";
            txtInputField3.text = "";
            txtInputField4.text = "";
            txtInputField5.text = "";
        }
    }
}