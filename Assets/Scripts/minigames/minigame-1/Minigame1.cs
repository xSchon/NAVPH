/* Management script for minigame-1 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Minigame1 : MonoBehaviour
{
    public GameObject brick;
    private GameObject activeBrick;
    private List<GameObject> placedBricks = new List<GameObject>();
    public float endScore = 7.0f;
    public string lostText;
    public string winText;
    private float score = 0f;
    public float numberOfBricks = 10f;
    public TMP_Text UIscore;
    public TMP_Text UIbricks;
    private bool endGame = false;
    public TMP_Text popup;
    public GameObject button;
    public Camera minigameCamera;
    public Canvas tutorial;
    public Image background;

    void Start()
    {
        popup.enabled = false;
        button.SetActive(false);
        background.enabled = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawNewBrick();
        }
        UpdateUI();

        IncrementScore();

    }

    // Spawns new brick at the top of screen
    public void SpawNewBrick()
    {
        IncrementScore();
        CheckScore();
        if (!endGame)
        {
            activeBrick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);

            numberOfBricks--;
        }
    }

    // Increment score
    public void IncrementScore()
    {
        float newScore = -0.5f;

        foreach (GameObject brickHeight in placedBricks)
        {
            if (brickHeight.transform.position.y > newScore)
            {
                newScore = brickHeight.transform.position.y;
            }
        }

        score = Mathf.Ceil(newScore + 0.4f);
    }

    // Check if the game didnt end
    private void CheckScore()
    {
        if (score >= endScore)
        {
            popup.enabled = true;
            button.SetActive(true);
            background.enabled = true;
            popup.text = winText;
            endGame = true;
            PlayerPrefs.SetInt("WonMinigame", 1);
            Time.timeScale = 0.0f;
        }

        if (numberOfBricks <= 0)
        {
            numberOfBricks--;
            popup.enabled = true;
            background.enabled = true;
            button.SetActive(true);
            popup.text = lostText;
            endGame = true;
            PlayerPrefs.SetInt("WonMinigame", 0);
        }
    }

    // Add newly placed brick to list of existing to recalculate new height
    public void AddPlacedBrick(GameObject brick)
    {
        placedBricks.Add(brick);
    }

    // Update UI with new values
    void UpdateUI()
    {
        UIscore.text = "Height: " + score;
        UIbricks.text = "Bricks: " + (numberOfBricks + 1);
    }

    public void EndTutorial()
    {
        tutorial.enabled = false;
    }
}
