using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    public Player opponent;

    public Dictionary<string, string[]> effectiveTypes;
    public Dictionary<string, string[]> vulnerableTypes;

    public string lastAttack;

    public double effectiveMultiplier = 1.5;

    void Start()
    {
        effectiveTypes = new Dictionary<string, string[]>();
        effectiveTypes.Add("Fire", new string[]{"Grass", "Fairy"});
        effectiveTypes.Add("Water", new string[]{"Fire", "Electric"});
        effectiveTypes.Add("Grass", new string[]{"Water", "Fairy"});
        effectiveTypes.Add("Electric", new string[]{"Grass", "Fire"});
        effectiveTypes.Add("Fairy", new string[]{"Water", "Electric"});

        vulnerableTypes = new Dictionary<string, string[]>();
        vulnerableTypes.Add("Fire", new string[]{"Water", "Electric"});
        vulnerableTypes.Add("Water", new string[]{"Grass", "Fairy"});
        vulnerableTypes.Add("Grass", new string[]{"Fire", "Electric"});
        vulnerableTypes.Add("Electric", new string[]{"Water", "Fairy"});
        vulnerableTypes.Add("Fairy", new string[]{"Fire", "Grass"});
    }

    public void setUpAI(Player opponent) {
        // TODO: change charmander to different pokemon
        string defaultEnemy = "charmander";
        ReadJson jsonReader = new ReadJson();
        ReadJson.Character chosenChar = jsonReader.LoadJson(defaultEnemy);
        initializeStats(defaultEnemy, chosenChar);
        this.opponent = opponent;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Decision tree
    public void getBestMove() {
        // if we're about to die
        if (canDieNextTurn()) {
            // check if the enemy is about to die
            if(isEnemyAboutToDie()) {
                // Perform KO attack

            } else {
                // Use HP, DEF, or SPD item, then attack with best attack

            }
        } else {
            // Use the best attack against enemy

        }
    }
    
    public int applyCrit(int currentDmg, bool usedCritItem) {
        int critPercent = 10;
        if(usedCritItem) {
            critPercent += 5;
        }
        if(UnityEngine.Random.Range(0,101) <= critPercent) {
            return (int) (currentDmg * 1.5);
        } else {
            return currentDmg;
        }
    }

    public int getBestAttackAgainstEnemy() {
        // if we are effective against enemy type
        if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
            // use special attack
            if(accuracyBoostItems > 0) {
                //use riskiest, highest dmg special
                accuracyBoostItems--;
                lastAttack = specialAttackNames[2];

                double newAccuracy = (specialAttackACC[2] * 1.1) - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    return applyCrit(currentDmg, false);
                } else {
                    return 0;   //attack misses
                }
            } else if(atkBoostItems > 0) {
                //use more reliable, 2nd highest dmg special
                atkBoostItems--;
                lastAttack = specialAttackNames[1];
                
                double newAccuracy = specialAttackACC[1] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[1] * effectiveMultiplier * ((double)ATK/opponent.DEF)) + 20;
                    return applyCrit(currentDmg, false);
                } else {
                    return 0;
                }
                
            } else if(critRateBoostItems > 0) {
                //use most accurate, least dmg special
                critRateBoostItems--;
                lastAttack = specialAttackNames[0];

                double newAccuracy = specialAttackACC[0] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[0] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    return applyCrit(currentDmg, true);
                } else {
                    return 0;
                }
            } else {
                // default to highest DMG special attack
                lastAttack = specialAttackNames[2];

                double newAccuracy = specialAttackACC[2] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    return applyCrit(currentDmg, false);
                } else {
                    return 0;   //attack misses
                }
            }
        } else {
            // use basic attack: default to highest basic attack, apply items if we can
            double newAccuracy = (basicAttackACC[2]); 
            int currentDmg = (int) (basicAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
            bool critBoost = false;

            if(accuracyBoostItems > 0) {
                accuracyBoostItems--;
                lastAttack = basicAttackNames[2];
                
                newAccuracy *= 1.1;
            } else if(atkBoostItems > 0) {
                atkBoostItems--;
                currentDmg += 20;
            } else if(critRateBoostItems > 0) {
                critBoost = true;
            }
            newAccuracy -= opponent.SPD;
            if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                return applyCrit(currentDmg, critBoost);
            } else {
                return 0;
            }
        }
    }

    public bool canDieNextTurn() {        
        // we are weak against enemy type, add a multiplier for enemy output damage for special attacks
        if(Array.IndexOf(vulnerableTypes[TYPE], opponent.TYPE) != -1) {
            for(int i = 0; i < opponent.specialAttackNames.Count; i++) {
                if(opponent.specialAttackDMG[i] * 1.5 >= HP) {
                    return true;
                }
            }
        } else {
            for(int i = 0; i < opponent.basicAttackNames.Count; i++) {
                if(opponent.basicAttackDMG[i] >= HP) {
                    return true;
                }
            }
            for(int i = 0; i < opponent.specialAttackNames.Count; i++) {
                if(opponent.specialAttackDMG[i] >= HP) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isEnemyAboutToDie() {        
        if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
            for(int i = 0; i < specialAttackNames.Count; i++) {
                if(specialAttackDMG[i] * 1.5 >= opponent.HP) {
                    return true;
                }
            }
        } else {
            for(int i = 0; i < basicAttackNames.Count; i++) {
                if(basicAttackDMG[i] >= opponent.HP) {
                    return true;
                }
            }
            for(int i = 0; i < specialAttackNames.Count; i++) {
                if(specialAttackDMG[i] >= opponent.HP) {
                    return true;
                }
            }
        }
        return false;
    }
}
