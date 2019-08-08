using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GameObject restartText;
    private static bool blinker = true;

    void Start()
    {
        StartCoroutine(WaitTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("Game");
            Score._scoreKills = 0;
        }
    }

    IEnumerator WaitTime()
    {
        restartText.SetActive(blinker);
        yield return new WaitForSeconds(.5f);
        blinker = !blinker;
        StartCoroutine(WaitTime());
    }
}
