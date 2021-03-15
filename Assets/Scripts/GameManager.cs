using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private int score, highScore;
    public Text scoreText, highScoreText;

    private float time;
    public Text timeText;

    public bool started, gameOver;

    public GameObject gameOverPanel;
    public Text gameOverScore, gameOverHighScore;
    public Button playAgain;
    public Button homeButton;

    void Start() {
      instance = GetComponent<GameManager>();

      playAgain.onClick.AddListener(Restart);
      homeButton.onClick.AddListener(goHome);

      score = 0;
      scoreText.text = "Score : " + score;
      highScore = PlayerPrefs.GetInt("HighScore");
      highScoreText.text = "Meilleur score : " + highScore;

      gameOver = false;
      UpdateTime();
      StartTimer();
    }

    void Update() {
      if(started) {
        time -= Time.deltaTime;
        UpdateTime();
        if(time <= 0) {
          GameOver();
        }
      }
    }

    public void IncreaseScore() {
      score++;
      scoreText.text = "Score : " + score;
      time += 1;
      UpdateTime();
    }

    public void StartTimer() {
      time = 30;
      started = true;
    }

    public void UpdateTime() {
      string minutes = Mathf.Floor(time / 60).ToString("00");
      string seconds = Mathf.Floor(time % 60).ToString("00");
      timeText.text = string.Format("Time : {0}:{1}", minutes, seconds);
    }

    private void GameOver() {
      time = 0;
      UpdateTime();
      started = false;
      gameOver = true;
      gameOverPanel.SetActive(true);
      gameOverScore.text = "Score final : " + score;
      if(score > highScore) {
        gameOverHighScore.text = "Meilleur score : " + score;
        PlayerPrefs.SetInt("HighScore", score);
        highScore = score;
      } else {
        gameOverHighScore.text = "Meilleur score : " + highScore;
      }
      bool success = ScoreAPI.sendScore(Manager.Instance.playerName, score);

    }

    private void Restart() {
      score = 0;
      scoreText.text = "Score : " + score;
      highScore = PlayerPrefs.GetInt("HighScore");
      highScoreText.text = "Meilleur score : " + highScore;

      gameOver = false;
      UpdateTime();
      StartTimer();

      gameOverPanel.SetActive(false);
    }

    private void goHome() {
      SceneManager.LoadScene("HomeScene");
    }

    public void loadGame() {
      SceneManager.LoadScene("GameScene");
    }
}
