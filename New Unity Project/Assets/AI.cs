using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    public Player opponent;
    public string lastAttack;
    public string[] characterNames;
    public string[] characterFileNames;

    void Start()
    {
        
    }

    public void setUpAI(Player opponent) {  
        Debug.Log("Inside setup AI");
        characterFileNames = new string[] { "recursive_snail", "wet_noodle", "joe", "gumdrop", "firefox" };
        characterNames = new string[] { "Recursive Snail", "Wet Noodle", "Joe", "Gumdrop", "Firefox" };    
        int index = UnityEngine.Random.Range(0,5);
        ReadJson jsonReader = new ReadJson();
        Debug.Log("Loading AI " + characterFileNames[index]);
        ReadJson.Character chosenChar = jsonReader.LoadJson(characterFileNames[index]);
        Debug.Log("Chosen AI name: " + characterNames[index]);
        initializeStats(characterNames[index], chosenChar);
        this.opponent = opponent;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Decision tree
    public int getBestMove() {
        // if we're about to die
        if (canDieNextTurn()) {
            // check if the enemy is about to die
            if(isEnemyAboutToDie()) {
                // Perform KO attack
                bool usedItem = applyHPDEFSPD();
                for(int i = 0; i < 3; i++) {
                    double basicDmg = basicAttackDMG[i];
                    if(basicDmg >= opponent.HP) {
                        //use basic
                        basicDmg *= ((double)ATK/opponent.DEF);
                        double newAccuracy = basicAttackACC[i] - opponent.SPD;
                        displayText.text += this.name + " used " + basicAttackNames[i] + ".\n";
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            Debug.Log("AI uses basic attack!");
                            return applyCrit(basicDmg, false);
                        } else {
                            displayText.text += this.name + " missed.\n";
                            Debug.Log("AI uses basic attack, but missed.");
                            return 0;
                        }
                    } else if(!usedItem && atkBoostItems > 0 && (basicDmg + 20 >= opponent.HP)) {
                        atkBoostItems--;
                        lastAttack = basicAttackNames[i];
                        displayText.text += this.name + " used the ATK boost item.\n";
                        displayText.text += this.name + " used " + basicAttackNames[i] + "!\n";
                        
                        double newAccuracy = basicAttackACC[i] - opponent.SPD;
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            basicDmg *= ((double)ATK/opponent.DEF);
                            basicDmg += 20;
                            Debug.Log("AI uses basic attack with ATK boost item!");
                            return applyCrit(basicDmg, false);
                        } else {
                            displayText.text += this.name + "missed.\n";
                            Debug.Log("AI uses basic attack with ATK boost item, but missed.");
                            return 0;
                        }
                    }
                    double specialDmg = specialAttackDMG[i];
                    bool effective = true;
                    if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
                        specialDmg = (specialDmg * effectiveMultiplier);
                    } else if(Array.IndexOf(vulnerableTypes[TYPE], opponent.TYPE) != -1) {
                        effective = false;
                        specialDmg *= (specialDmg * ineffectiveMultiplier);
                    }

                    if(specialDmg >= opponent.HP) {
                        //use special
                        specialDmg *= ((double)ATK/opponent.DEF);
                        displayText.text += this.name + " used " + specialAttackNames[i] + "!\n";
                        if(effective) {
                            displayText.text += "It's super effective!\n";
                        } else {
                            displayText.text += "It's not very effective.\n";
                        }
                        return applyCrit(specialDmg, false); 
                    } else if(!usedItem && atkBoostItems > 0 && (specialDmg+20 >= opponent.HP)) {
                        atkBoostItems--;
                        lastAttack = specialAttackNames[i];
                        displayText.text += this.name + " used the ATK boost item.\n";
                        displayText.text += this.name + " used " + specialAttackNames[i] + "!\n";
                        
                        double newAccuracy = specialAttackACC[i] - opponent.SPD;
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            specialDmg *= ((double)ATK/opponent.DEF);
                            specialDmg += 20.0;
                            Debug.Log("AI uses special attack!");
                            return applyCrit(specialDmg, false);
                        } else {
                            displayText.text += specialAttackNames[i] + "missed.\n";
                            Debug.Log("AI uses special attack, but missed.");
                            return 0;
                        }
                    }
                }
                return getBestAttackAgainstEnemy(usedItem);
            } else {
                // Use HP, DEF, or SPD item, then attack with best attack
                bool usedItem = applyHPDEFSPD();
                return getBestAttackAgainstEnemy(usedItem);
            }
        } else {
            // Use the best attack against enemy
            return getBestAttackAgainstEnemy(false);
        }
    }

    public bool applyHPDEFSPD() {
        if(healItems > 0) {
            healItems--;
            int heal = 50;
            if((double) HP < (maxHP * .25)) {
                heal += 20;
            }
            HP = Math.Min(maxHP, HP + heal);
            displayText.text = "Used heal item!\n";;
            Debug.Log("AI used heal item! HP is now "+HP);
            return true;
        } else if (defBoostItems > 0) {
            defBoostItems--;
            DEF += 25;
            displayText.text = "Used DEF boost item!\n";
            Debug.Log("AI used DEF boost item! DEF is now "+DEF);
            return true;
        } else if (spdBoostItems > 0) {
            spdBoostItems--;
            SPD += 10;
            displayText.text = "Used SPD item!\n";
            Debug.Log("AI used SPD boost item! SPD is now "+SPD);
            return true;
        }
        return false;
    }

    public int getBestAttackAgainstEnemy(bool usedItem) {
        // if we are effective against enemy type
        if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
            // use special attack
            if(!usedItem && accBoostItems > 0) {
                //use riskiest, highest dmg special
                accBoostItems--;
                lastAttack = specialAttackNames[2];
                displayText.text += this.name + " used the accuracy boost item.\n";
                displayText.text += this.name + " used " + specialAttackNames[2] + ".\n";
                double newAccuracy = (specialAttackACC[2] * 1.1) - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    displayText.text += "It's super effective!\n";
                    Debug.Log("AI used strongest special attack with accuracy boost item!");
                    return applyCrit(currentDmg, false);
                } else {
                    displayText.text += specialAttackNames[2] + " missed.\n";
                    Debug.Log("AI used strongest special attack with accuracy boost item, but missed.");
                    return 0;   //attack misses
                }
            } else if(!usedItem && atkBoostItems > 0) {
                //use more reliable, 2nd highest dmg special
                atkBoostItems--;
                lastAttack = specialAttackNames[1];
                displayText.text += this.name + " used the ATK boost item.\n";
                displayText.text += this.name + " used " + specialAttackNames[1] + ".\n";

                double newAccuracy = specialAttackACC[1] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    displayText.text += "It's super effective!\n";
                    int currentDmg = (int) (specialAttackDMG[1] * effectiveMultiplier * ((double)ATK/opponent.DEF)) + 20;
                    Debug.Log("AI used second strongest special attack with ATK boost item!");
                    return applyCrit(currentDmg, false);
                } else {
                    displayText.text += specialAttackNames[1] + " missed.\n";
                    Debug.Log("AI used second strongest special attack with ATK boost item but missed.");
                    return 0;
                }
                
            } else if(!usedItem && critBoostItems > 0) {
                //use most accurate, least dmg special
                critBoostItems--;
                lastAttack = specialAttackNames[0];
                displayText.text += this.name + " used the Crit Rate boost item.\n";
                displayText.text += this.name + " used " + specialAttackNames[0] + ".\n";
                
                double newAccuracy = specialAttackACC[0] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    displayText.text += "It's super effective!\n";
                    int currentDmg = (int) (specialAttackDMG[0] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    Debug.Log("AI used weakest special attack along with CRIT boost item!");
                    return applyCrit(currentDmg, true);
                } else {
                    displayText.text += specialAttackNames[0] + " missed.\n";
                    Debug.Log("AI used weakest special attack along with CRIT boost item, but missed.");
                    return 0;
                }
            } else {
                // default to highest DMG special attack
                lastAttack = specialAttackNames[2];
                displayText.text += this.name + " used " + specialAttackNames[2] + ".\n";

                double newAccuracy = specialAttackACC[2] - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    displayText.text += "It's super effective!\n";
                    int currentDmg = (int) (specialAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    return applyCrit(currentDmg, false);
                } else {
                    displayText.text += specialAttackNames[0] + " missed.\n";
                    return 0;   //attack misses
                }
            }
        } else {
            // use basic attack: default to highest basic attack, apply items if we can
            double newAccuracy = (basicAttackACC[2]); 
            int currentDmg = (int) (basicAttackDMG[2] * ((double)ATK/opponent.DEF));
            bool critBoost = false;

            if(!usedItem && accBoostItems > 0) {
                accBoostItems--;
                lastAttack = basicAttackNames[2];
                displayText.text += this.name + " used the accuracy boost item.\n";
                
                newAccuracy *= 1.1;
                Debug.Log("AI used accuracy boost item!");
            } else if(!usedItem && atkBoostItems > 0) {
                atkBoostItems--;
                currentDmg += 20;

                displayText.text += this.name + " used the ATK boost item.\n";
                Debug.Log("AI used ATK boost item!");
            } else if(!usedItem && critBoostItems > 0) {
                displayText.text += this.name + " used the crit boost item.\n";
                critBoost = true;
                Debug.Log("AI used CRIT boost item!");
            }
            displayText.text += this.name + " used " + basicAttackNames[2] + ".\n";
            
            newAccuracy -= opponent.SPD;
            if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                Debug.Log("AI uses strongest basic attack!");
                return applyCrit(currentDmg, critBoost);
            } else {
                displayText.text += basicAttackNames[2] + " missed.\n";
                Debug.Log("AI uses strongest basic attack, but missed.");
                return 0;
            }
        }
    }

    public bool canDieNextTurn() {     
        bool res = Array.IndexOf(vulnerableTypes[TYPE], opponent.TYPE) != -1;  
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
            if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
                for(int i = 0; i < specialAttackNames.Count; i++) {
                    if((specialAttackDMG[i]*ineffectiveMultiplier) >= opponent.HP) {
                        return true;
                    }
                }
            } else  {
                for(int i = 0; i < specialAttackNames.Count; i++) {
                    if(specialAttackDMG[i] >= opponent.HP) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
