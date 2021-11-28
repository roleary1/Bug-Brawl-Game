using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleScene : MonoBehaviour
{

    public Button basicAttack1, basicAttack2, basicAttack3, specialAttack1, specialAttack2, specialAttack3;
    public Button attackWindowButton, itemWindowButton;

    public Button item1, item2, item3, item4, item5, item6;

    public Image attackWindow, itemWindow;
    public Text playerName;
    public Text enemyName;
    public Text playerHP;
    public Text enemyHP;
    public Image playerImage;
    public Image enemyImage;
    public Player player;

    public AI enemy;

    public bool usedItem = false;
    public bool accBoost = false;
    public bool critBoost = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerGameObj = GameObject.Find("Player");
        if (playerGameObj != null) {
            player = playerGameObj.GetComponent<Player>();
            Debug.Log("Player name is: " + player.name);
            Debug.Log("ATK: "+ player.ATK);
            Debug.Log("HP: "+ player.HP);
        } else {
            Debug.Log("Player game obj was null");
        }

        GameObject enemyGameObj = GameObject.Find("Enemy");
        if (enemyGameObj != null) {
            enemy = enemyGameObj.GetComponent<AI>();
            Debug.Log("Enemy name is: " + enemy.name);
            Debug.Log("ATK: "+ enemy.ATK);
            Debug.Log("HP: "+ enemy.HP);
        } else {
            Debug.Log("Enemy game obj was null");
        }
        player.setUpDict();
        enemy.setUpDict();
        
        playerName.text = player.name;
        enemyName.text = enemy.name;

        enemyHP.text = "HP: "+ enemy.maxHP;
        playerHP.text = "HP: "+ player.maxHP;

        playerImage.sprite = Resources.Load <Sprite> (player.name);
        enemyImage.sprite = Resources.Load <Sprite> (enemy.name);
        
        basicAttack1.onClick.AddListener(delegate { executeAttack(0); });
        basicAttack2.onClick.AddListener(delegate { executeAttack(1); });
        basicAttack3.onClick.AddListener(delegate { executeAttack(2); });
        specialAttack1.onClick.AddListener(delegate { executeAttack(3); });
        specialAttack2.onClick.AddListener(delegate { executeAttack(4); });
        specialAttack3.onClick.AddListener(delegate { executeAttack(5); });


        attackWindowButton.onClick.AddListener(delegate { changeTab("attack"); });
        itemWindowButton.onClick.AddListener(delegate { changeTab("items"); });

        item1.onClick.AddListener(delegate { useItem(0); });
        item2.onClick.AddListener(delegate { useItem(1); });
        item3.onClick.AddListener(delegate { useItem(2); });
        item4.onClick.AddListener(delegate { useItem(3); });
        item5.onClick.AddListener(delegate { useItem(4); });
        item6.onClick.AddListener(delegate { useItem(5); });
    }

    void Update() {

    }

    void executeAttack(int attack) {
        // get dmg from attack selected
        // apply dmg to AI
        // check if AI died
        // get AI response
        // apply AI dmg to player
        // check if we died
        // reset the AI and player stats to base stats, reset usedItem
        Debug.Log("Our Turn:");
        int totalDmg = 0;
        if(attack < 3) {
            Debug.Log("Basic Attack " + attack);
            int newAccuracy = player.basicAttackACC[attack];
            if(accBoost) {
                newAccuracy = (int)(newAccuracy * 1.1);
            }
            newAccuracy -= enemy.SPD;
            if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                totalDmg = (int) (player.basicAttackDMG[attack] * ((double)player.ATK/enemy.DEF));
                if(critBoost) {
                    totalDmg = player.applyCrit(totalDmg, true);
                } else {
                    totalDmg = player.applyCrit(totalDmg, false);
                }
            } else {
                Debug.Log("We missed :(");
            }
        } else {
            attack -= 3;
            Debug.Log("Special Attack " + attack);
            int newAccuracy = player.specialAttackACC[attack];
            if(accBoost) {
                newAccuracy = (int)(newAccuracy * 1.1);
            }
            newAccuracy -= enemy.SPD;
            if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                totalDmg = (int) (player.specialAttackDMG[attack] * ((double)player.ATK/enemy.DEF));
                if(Array.IndexOf(player.effectiveTypes[player.TYPE], enemy.TYPE) != -1) {
                    totalDmg = (int) (totalDmg * player.effectiveMultiplier);
                }
                if(Array.IndexOf(player.vulnerableTypes[player.TYPE], enemy.TYPE) != -1) {
                    totalDmg = (int) (totalDmg * player.ineffectiveMultiplier);
                }
                if(critBoost) {
                    totalDmg = player.applyCrit(totalDmg, true);
                } else {
                    totalDmg = player.applyCrit(totalDmg, false);
                }
            } else {
                Debug.Log("We missed :(");
            }
        }
        
        Debug.Log("Apply Player Damage: " + totalDmg);
        enemy.HP -= totalDmg;
        enemyHP.text = "HP: " + enemy.HP;
        if(enemy.HP <= totalDmg) {
            Debug.Log("We KOed the Enemy!");
            // we killed the enemy with our attack
            // print we won, change scene to wining screen

        } 
        // we didn't kill them
        Debug.Log("Enemy Turn");
        int enemyDmg = enemy.getBestMove();
        player.HP -= enemyDmg;
        playerHP.text = "HP: " + player.HP;
        if(player.HP <= enemyDmg) {
            Debug.Log("Enemy KOed us :(");
            // we died, take us to lose screen

        }
        // no one died, reset the stats for enemy and us
        Debug.Log("Reseting stats for everyone");
        accBoost = false;
        critBoost = false;
        usedItem = false;
        player.DEF = player.baseDEF;
        player.SPD = player.baseSPD;
        player.ATK = player.baseATK;
        
        enemy.DEF = enemy.baseDEF;
        enemy.SPD = enemy.baseSPD;
        enemy.ATK = enemy.baseATK;
    }

    void useItem(int item) {
        switch(item) {
            case 0:
                // Heal
                if(player.healItems > 0 && !usedItem) {
                    player.healItems--;
                    int heal = 50;
                    if((double) player.HP < (player.maxHP * .25)) {
                        heal += 20;
                    }
                    player.HP = Math.Min(player.maxHP, player.HP + heal);
                }
                Debug.Log("Used heal: HP is now "+ player.HP);
                break;
            case 1:
                // DEF boost
                if(player.defBoostItems > 0 && !usedItem) {
                    player.defBoostItems--;
                    player.DEF += 25;
                }
                Debug.Log("Used DEF boost: DEF is now " + player.DEF);
                break;
            case 2:
                // SPD boost
                if(player.spdBoostItems > 0 && !usedItem) {
                    player.spdBoostItems--;
                    player.SPD += 10;
                }
                Debug.Log("Used SPD boost: SPD is now " + player.SPD);
                break;
            case 3:
                // ATK boost
                if(player.atkBoostItems > 0 && !usedItem) {
                    player.atkBoostItems--;
                    player.ATK += 20;
                }
                Debug.Log("Used ATK boost: ATK is now "+ player.ATK);
                break;
            case 4:
                Debug.Log("Used ACC boost");
                // ACC boost
                if(player.accBoostItems > 0 && !usedItem) {
                    player.accBoostItems--;
                    accBoost = true;
                }
                break;
            case 5:
                Debug.Log("Used Crit boost");
                // Crit Rate boost
                if(player.critBoostItems > 0 && !usedItem) {
                    player.critBoostItems--;
                    critBoost = true;
                }
                break;
        }
        usedItem = true;
    }


    void changeTab(string tab) {
        Debug.Log(tab);
        if(tab == "attack") {
            for (int i = 0; i < itemWindow.transform.childCount; i++){
                itemWindow.transform.GetChild(i).gameObject.SetActive(false);
            }
            itemWindow.enabled = false;
            for (int i = 0; i < attackWindow.transform.childCount; i++){
                attackWindow.transform.GetChild(i).gameObject.SetActive(true);
            }
            attackWindow.enabled = true;
        } else {
            for (int i = 0; i < attackWindow.transform.childCount; i++){
                attackWindow.transform.GetChild(i).gameObject.SetActive(false);
            }
            attackWindow.enabled = false;
            for (int i = 0; i < itemWindow.transform.childCount; i++){
                itemWindow.transform.GetChild(i).gameObject.SetActive(true);
            }
            itemWindow.enabled = true;
        }
    }
}
