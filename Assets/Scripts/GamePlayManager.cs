using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]private InputField UsernameText;
    [SerializeField]private Button ReadyBtn;
    [SerializeField] private Button BtnStart;
    [SerializeField] private Button BtnQuit;
    private GameObject PanelBack;

    private void Start()
    {
        PanelBack = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        ReadyBtn.onClick.AddListener(() => ButtonClicked());
    }

    void ButtonClicked()
    {
        //1st
        Score._userName = UsernameText.text;
        //HideUsernamePanel
        ReadyBtn.gameObject.SetActive(false);
        UsernameText.gameObject.SetActive(false);
        PanelBack.gameObject.SetActive(false);
        //2nd
        //setBtnFunctions
        BtnStart.onClick.AddListener(() => BtnStartFunction());
        BtnQuit.onClick.AddListener(() => BtnQuitFunction());
        //ShowOptions
        BtnStart.gameObject.SetActive(true);
        BtnQuit.gameObject.SetActive(true);
    }

    void BtnStartFunction()
    {
        SceneManager.LoadScene("Game");
    }

    void BtnQuitFunction()
    {
        Application.Quit();
    }
}
