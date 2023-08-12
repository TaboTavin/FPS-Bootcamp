using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI timerText;

    private int score;
    private int bullets;
    private int life;
    private string position;
    private float timer;

    private void Start()
    {
        score = 0;
        bullets = 0;
        life = 100;
        position = "Normal";
        timer = 0f;
    }

    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        bulletsText.text = "Bullets: " + bullets.ToString();
        lifeText.text = "Life: " + life.ToString();
        positionText.text = "Position: " + position;
        timerText.text = "Timer: " + timer.ToString("F2") + "s";
    }

    public void UpdatePosition(string newPosition)
    {
        position = newPosition;
    }
}
