using UnityEngine;
using MVC.Controller;


public class GameManager : MonoBehaviour
{
    private void Start()
    {
        LoginController.Instance.ShowUI();
    }
}