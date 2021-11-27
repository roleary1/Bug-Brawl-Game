using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    public string opponentType;
    public Dictionary<string, string[]> effectiveTypes;

    // Start is called before the first frame update
    void Start()
    {
        effectiveTypes = new Dictionary<string, string[]>();
        effectiveTypes.Add("Fire", new string[]{"Grass", "Electric"});
        effectiveTypes.Add("Water", new string[]{"Fire", "Electric"});
        effectiveTypes.Add("Electric", new string[]{"Water", "Fairy"});
        effectiveTypes.Add("Fairy", new string[]{"Grass", "Fire"});
        effectiveTypes.Add("Grass", new string[]{"Water", "Fairy"});
    }

    public void setUpAI(string opponentType) {
        string defaultEnemy = "charmander";
        ReadJson jsonReader = new ReadJson();
        ReadJson.Character chosenChar = jsonReader.LoadJson(defaultEnemy);
        initializeStats(defaultEnemy,chosenChar);
        this.opponentType = opponentType;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string getBestAttack() {
        //check if we're effective against enemy
        if(Array.IndexOf(effectiveTypes[this.TYPE]) != -1) {
            if(accBoostItems > 0) {
                //if have accuracy boosts use the most dmg, least accuracy move
                accBoostItems--;

            } else if(atkBoostItems > 0) {
                //if have attack dmg boosts use the 2nd most dmg, 2nd most accuracy move
                atkBoostItems--;

            } else if(critBoostItems > 0) {
                //if have crit-rate boosts, use the least dmg, most accuracy move
                critBoostItems--;
            }
        }
    }
}
