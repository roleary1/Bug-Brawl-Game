using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string name;
    public int HP;
    public int ATK;
    public int DEF;
    public int SPD;
    public string TYPE;
    public List<string> basicAttackNames;
    public List<int> basicAttackDMG;
    public List<int> basicAttackACC;
    public List<string> specialAttackNames;
    public List<int> specialAttackDMG;
    public List<int> specialAttackACC;
    public Text playerHPDisplay;
    // Heal 50, if below 25% health add extra 20
    public int healItems = 2;
    // Increase DEF by 25
    public int defBoostItems = 3;
    // Increase ATK by 20
    public int atkBoostItems = 2;
    // Increase Accuracy of next attack by 10%
    public int accBoostItems = 3;
    // Increase SPD by 10
    public int spdBoostItems = 3;
    // Increase crit rate by 5%
    public int critBoostItems = 3;

    public bool gameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStart) {
            if(playerHPDisplay != null) {
                playerHPDisplay.text = "" + HP;
            }
        }
    }

    public void initializeStats(string playerName, ReadJson.Character charStats) {
        this.gameStart = true;
        
        this.name = playerName;
        this.HP = charStats.HP;
        this.ATK = charStats.ATK;
        this.DEF = charStats.DEF;
        this.SPD = charStats.SPD;
        this.TYPE = charStats.TYPE;

        this.basicAttackNames = charStats.basicAttackNames;
        this.basicAttackDMG = charStats.basicAttackDMG;
        this.basicAttackACC = charStats.basicAttackACC;
        this.specialAttackNames = charStats.specialAttackNames;
        this.specialAttackDMG = charStats.specialAttackDMG;
        this.specialAttackACC = charStats.specialAttackACC;
    }
}
