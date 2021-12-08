using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectScreen : MonoBehaviour
{
    public Button wet_noodle_button, firefox_button, joe_button, recursive_snail_button, gumdrop_button, battle_button, back_button;
    public ReadJson jsonReader;
    public Image player1;
    public GameObject playerGameObj;
    public Player playerObj;
    public GameObject enemyGameObj;
    public AI enemyObj;
    public Text statWindow;
    public string globalCharName;
    public ReadJson.Character chosenChar;

    public bool selectedCharacter = false;

    // Start is called before the first frame update
    void Start()
    {
        wet_noodle_button.onClick.AddListener(delegate { selectCharacter("wet_noodle"); });
        firefox_button.onClick.AddListener(delegate { selectCharacter("firefox"); });
        joe_button.onClick.AddListener(delegate { selectCharacter("joe"); });
        recursive_snail_button.onClick.AddListener(delegate { selectCharacter("recursive_snail"); });
        gumdrop_button.onClick.AddListener(delegate { selectCharacter("gumdrop"); });
        battle_button.onClick.AddListener(enterBattle);
        back_button.onClick.AddListener(backButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void backButtonPressed() {
        SceneManager.LoadScene("StartMenu");
    }

    void startNewBattle() {
        playerGameObj = new GameObject();
        playerGameObj.AddComponent<Player>();
        playerGameObj.name = "Player";
        playerObj = playerGameObj.GetComponent<Player>();

        enemyGameObj = new GameObject();
        enemyGameObj.AddComponent<AI>();
        enemyGameObj.name = "Enemy";
        enemyObj = enemyGameObj.GetComponent<AI>();
    }

    void enterBattle()
    {
        if(selectedCharacter) {
            startNewBattle();
            // Set up the player stats before transitioning
            Debug.Log("Initalizing character " + globalCharName);
            playerObj.initializeStats(globalCharName, chosenChar);
            enemyObj.setUpAI(playerObj);
            SceneManager.LoadScene("BattleScene");
        }
    } 

    void selectCharacter(string characterName)
    {
        string[] characterNames = new string[] { "Recursive Snail", "Wet Noodle", "Joe", "Gumdrop", "Firefox" };
        // string charName = "";
        globalCharName = characterName;
        switch (characterName) {
            case "wet_noodle":
                globalCharName = characterNames[1];
                break;
            case "recursive_snail":
                globalCharName = characterNames[0];
                break;
            case "joe":
                globalCharName = characterNames[2];
                break;
            case "gumdrop":
                globalCharName = characterNames[3];
                break;
            case "firefox":
                globalCharName = characterNames[4];
                break;
        }
        selectedCharacter = true;
        Debug.Log("Character chosen: " + globalCharName);
        
        jsonReader = new ReadJson();
        chosenChar = jsonReader.LoadJson(characterName);
        player1.sprite = Resources.Load <Sprite> (characterName);

        string statsToDisplay = "" + globalCharName + " Stats: \n" +
                                "HP: " + chosenChar.HP + ", " +
                                "ATK: " + chosenChar.ATK + ", " +
                                "DEF: " + chosenChar.DEF + ", " +
                                "SPD: " + chosenChar.SPD + ", " +
                                "TYPE: " + chosenChar.TYPE + " \n" + 
                                "Basic Attacks: \n";
        for (int i = 0; i < chosenChar.basicAttackNames.Count; i++) {
            statsToDisplay += chosenChar.basicAttackNames[i] + " => Attack Dmg: " + chosenChar.basicAttackDMG[i] + ", Attack Accuracy: " + chosenChar.basicAttackACC[i] + "\n";
        }

        statsToDisplay += "Special Attacks: \n";

        for (int i = 0; i < chosenChar.specialAttackNames.Count; i++) {
            statsToDisplay += chosenChar.specialAttackNames[i] + " => Attack Dmg: " + chosenChar.specialAttackDMG[i] + ", Attack Accuracy: " + chosenChar.specialAttackACC[i] + "\n";
        }

        statWindow.text = statsToDisplay;
    }
}
