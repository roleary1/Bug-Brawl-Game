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
    public int healItems = 5;
    public int defBoostItems = 5;
    public int atkBoostItems = 5;

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
