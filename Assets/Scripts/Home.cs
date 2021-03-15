using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
  public Text scoreboardText;
  public InputField nameInput;
  public Button playButton;

    void Start()
    {

      if(PlayerPrefs.HasKey("name")){
        nameInput.text = PlayerPrefs.GetString("name");
      }

      playButton.onClick.AddListener(Play);

      Scoreboard scoreboard = ScoreAPI.getScoreboard();
      string message = "Tableau des scores : \n\n Chargement...";
      scoreboardText.text = message;
      if(scoreboard != null) {
        message = "Tableau des scores : \n\n";
        for (int i = 0;i < scoreboard.Length();i++) {
          message += (i+1) + ") " + scoreboard.getScore(i).name + " : " + scoreboard.getScore(i).score + "\n\n";
          if(i == 4) break;
        }
      } else {
        message = "Problème de réseau";
      }

      scoreboardText.text = message;
    }

    public void Play() {
      string name = nameInput.text;
      Manager.Instance.playerName = "Anonymous";
      if(name != "") {
        Manager.Instance.playerName = name;
        PlayerPrefs.SetString("name", name);
      }
      SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
