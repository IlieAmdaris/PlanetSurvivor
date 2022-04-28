using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDialog : MonoBehaviour
{
    public KnightBehaviour knightBehaviour;
    public HUD hud;
    public Text gameOverText;
    void Start()
    {
        gameObject.SetActive(false);

    }
    void Update()
    {
        if (knightBehaviour.isGameOver)
        {
            gameObject.SetActive(true);
            gameOverText.text = $"You played for: {knightBehaviour.timePlayed} seconds";
        }
    }
    public void RestartDialog()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
