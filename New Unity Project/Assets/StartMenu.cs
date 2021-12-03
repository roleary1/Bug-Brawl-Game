using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Button startButton, exitButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(delegate { startButtonPressed();});
        exitButton.onClick.AddListener(delegate { exitButtonPressed();});
    }

    void startButtonPressed() {
        SceneManager.LoadScene("MainScene");
    }

    void exitButtonPressed() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
