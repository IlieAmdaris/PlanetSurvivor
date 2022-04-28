using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameOverDialog gameOverDialog;
    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        gameOverDialog.gameObject.SetActive(false);
    }
    public void showGameOverDialog()
    {
        gameOverDialog.gameObject.SetActive(true);
    }
}
