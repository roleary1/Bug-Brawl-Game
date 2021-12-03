using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectScreen : MonoBehaviour
{
    public Button squirtle_button, charmander_button, bulbasaur_button, pikachu_button, jiggly_button, battle_button, back_button;
    public ReadJson jsonReader;
    public Image player1;
    public GameObject playerGameObj;
    public Player playerObj;
    public GameObject enemyGameObj;
    public AI enemyObj;
    public Text statWindow;
    public string charName;
    public ReadJson.Character chosenChar;

    public bool selectedCharacter = false;

    // Start is called before the first frame update
    void Start()
    {
        squirtle_button.onClick.AddListener(delegate { selectCharacter("squirtle"); });
        charmander_button.onClick.AddListener(delegate { selectCharacter("charmander"); });
        bulbasaur_button.onClick.AddListener(delegate { selectCharacter("bulbasaur"); });
        pikachu_button.onClick.AddListener(delegate { selectCharacter("pikachu"); });
        jiggly_button.onClick.AddListener(delegate { selectCharacter("jigglypuff"); });
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
            Debug.Log("Initalizing character " + charName);
            playerObj.initializeStats(charName, chosenChar);
            enemyObj.setUpAI(playerObj);
            SceneManager.LoadScene("BattleScene");
        }
    } 

    void selectCharacter(string characterName)
    {
        selectedCharacter = true;
        charName = char.ToUpper(characterName[0]) + characterName.Substring(1);
        Debug.Log("Character chosen: " + charName);
        
        jsonReader = new ReadJson();
        chosenChar = jsonReader.LoadJson(characterName);
        player1.sprite = Resources.Load <Sprite> (characterName);

        string statsToDisplay = "" + char.ToUpper(characterName[0]) + characterName.Substring(1) + " Stats: \n" +
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
