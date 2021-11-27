using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    public Player opponent;
    public string lastAttack;

    //public Text enemyHPDisplay;

    void Start()
    {
       
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
    public int getBestMove() {
        // if we're about to die
        if (canDieNextTurn()) {
            // check if the enemy is about to die
            if(isEnemyAboutToDie()) {
                // Perform KO attack
                bool usedItem = applyHPDEFSPD();
                for(int i = 0; i < 3; i++) {
                    int basicDmg = basicAttackDMG[i];
                    if(basicDmg >= opponent.HP) {
                        //use basic
                        basicDmg *= (int) ((double)ATK/opponent.DEF);
                        double newAccuracy = basicAttackACC[i] - opponent.SPD;
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            return applyCrit(basicDmg, false);
                        } else {
                            return 0;
                        }
                    } else if(!usedItem && atkBoostItems > 0 && (basicDmg+20 >= opponent.HP)) {
                        atkBoostItems--;
                        lastAttack = basicAttackNames[i];
                
                        double newAccuracy = basicAttackACC[i] - opponent.SPD;
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            basicDmg *= (int) ((double)ATK/opponent.DEF);
                            basicDmg += 20;
                            return applyCrit(basicDmg, false);
                        } else {
                            return 0;
                        }
                    }
                    int specialDmg = specialAttackDMG[i];
                    if(Array.IndexOf(effectiveTypes[TYPE], opponent.TYPE) != -1) {
                        specialDmg = (int) (specialDmg * effectiveMultiplier);
                    } else if(Array.IndexOf(vulnerableTypes[TYPE], opponent.TYPE) != -1) {
                        specialDmg *= (int) (specialDmg * ineffectiveMultiplier);;
                    }

                    if(specialDmg >= opponent.HP) {
                        //use special
                        specialDmg *= (int) ((double)ATK/opponent.DEF);
                        return applyCrit(specialDmg, false); 
                    } else if(!usedItem && atkBoostItems > 0 && (specialDmg+20 >= opponent.HP)) {
                        atkBoostItems--;
                        lastAttack = specialAttackNames[i];
                
                        double newAccuracy = specialAttackACC[i] - opponent.SPD;
                        if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                            specialDmg *= (int) ((double)ATK/opponent.DEF);
                            specialDmg += 20;
                            return applyCrit(specialDmg, false);
                        } else {
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
            return true;
        } else if (defBoostItems > 0) {
            defBoostItems--;
            DEF += 25;
            return true;
        } else if (spdBoostItems > 0) {
            spdBoostItems--;
            SPD += 10;
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

                double newAccuracy = (specialAttackACC[2] * 1.1) - opponent.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    int currentDmg = (int) (specialAttackDMG[2] * effectiveMultiplier * ((double)ATK/opponent.DEF));
                    return applyCrit(currentDmg, false);
                } else {
                    return 0;   //attack misses
                }
            } else if(!usedItem && atkBoostItems > 0) {
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
                
            } else if(!usedItem && critBoostItems > 0) {
                //use most accurate, least dmg special
                critBoostItems--;
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
            int currentDmg = (int) (basicAttackDMG[2] * ((double)ATK/opponent.DEF));
            bool critBoost = false;

            if(!usedItem && accBoostItems > 0) {
                accBoostItems--;
                lastAttack = basicAttackNames[2];
                
                newAccuracy *= 1.1;
            } else if(!usedItem && atkBoostItems > 0) {
                atkBoostItems--;
                currentDmg += 20;
            } else if(!usedItem && critBoostItems > 0) {
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
