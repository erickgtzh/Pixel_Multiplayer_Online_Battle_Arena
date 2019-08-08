using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    //[SerializeField] private TextMesh _scoreText;
    private Text killsText;
    private Text deathsText;
    private Text usernameText;

    private void Start()
    {
        killsText = GameObject.Find("TextKills").GetComponent<Text>();
        deathsText = GameObject.Find("TextDeaths").GetComponent<Text>();
        usernameText = GameObject.Find("TextPlayerName").GetComponent<Text>();
    }

    private void Update()
    {
        killsText.text = "Kills: "+Score.GetKills();
        deathsText.text = "Deaths: " + Score.GetDeaths();
        usernameText.text = Score.GetUsername();
    }
}
