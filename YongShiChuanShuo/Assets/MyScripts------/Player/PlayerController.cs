using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;//用于读取PlayerInputControl的MOve里的Vector2值

    private void Awake()
    {
        inputControl = new PlayerInputControl(); //实例化PlayerInputControl类
    }
    void OnEnable()
    {
        inputControl.Enable(); //启用PlayerInputControl类
    }
    void OnDisable()
    {
        inputControl.Dispose(); //销毁PlayerInputControl类
    }
}
