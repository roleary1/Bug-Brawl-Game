using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScreen : MonoBehaviour
{

    public Button squirtle_button, charmander_button, bulbasaur_button, pikachu_button, jiggly_button;
    public ReadJson jsonReader;
    //public ReadJson.Character chosenChar;
    public Image player1;

    public Text statWindow;

    // Start is called before the first frame update
    void Start()
    {
        squirtle_button.onClick.AddListener(delegate { selectCharacter("squirtle"); });
        charmander_button.onClick.AddListener(delegate { selectCharacter("charmander"); });
        bulbasaur_button.onClick.AddListener(delegate { selectCharacter("bulbasaur"); });
        pikachu_button.onClick.AddListener(delegate { selectCharacter("pikachu"); });
        jiggly_button.onClick.AddListener(delegate { selectCharacter("jigglypuff"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void selectCharacter(string characterName)
    {
        Debug.Log(characterName);
        jsonReader = new ReadJson();
        ReadJson.Character chosenChar = jsonReader.LoadJson(characterName);
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
