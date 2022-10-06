using System;
using MVC.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVC.View
{
    public class HandCard : BaseView, ISelectHandler, IDeselectHandler
    {
        public byte card;
        private bool isSelected;
        public event Action<byte, GameObject> onPlayCard;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnSelect(BaseEventData eventData)
        {
            transform.localPosition += 20 * Vector3.up;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            transform.localPosition += 20 * Vector3.down;
            isSelected = false;
        }

        private void OnClick()
        {
            if (isSelected)
                onPlayCard?.Invoke(card, gameObject);
            else
                isSelected = true;
        }
    }
}