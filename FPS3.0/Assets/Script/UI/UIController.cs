using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text text1; // 第一个Text
    public Text text2; // 第二个Text

    private bool isTextVisible; // 文本是否可见

    void Start()
    {
        HideText(); // 隐藏文本
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isTextVisible)
                HideText();
            else
                ShowText();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReturnToMainMenu();
        }
    }

    void ShowText()
    {
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        isTextVisible = true;
    }

    void HideText()
    {
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        isTextVisible = false;
    }

    void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None; // 解锁鼠标
        Cursor.visible = true; // 使鼠标可见
        SceneManager.LoadScene(0); // 加载主菜单场景
    }
}
