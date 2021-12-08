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

    public Animator playerAnimation;
    public Animator enemyAnimation;

    public Player player;

    public Text displayText;

    public AI enemy;

    public bool usedItem = false;
    public bool accBoost = false;
    public bool critBoost = false;

    public bool enemyDecidingMove = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Inside battle scene start");
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
        // set up animation


        player.setUpDict(displayText);
        enemy.setUpDict(displayText);
        
        playerName.text = player.name;
        enemyName.text = enemy.name;

        basicAttack1.GetComponentInChildren<Text>().text = player.basicAttackNames[0];
        specialAttack1.GetComponentInChildren<Text>().text = player.specialAttackNames[0];

        basicAttack2.GetComponentInChildren<Text>().text = player.basicAttackNames[1];
        specialAttack2.GetComponentInChildren<Text>().text = player.specialAttackNames[1];

        basicAttack3.GetComponentInChildren<Text>().text = player.basicAttackNames[2];
        specialAttack3.GetComponentInChildren<Text>().text = player.specialAttackNames[2];

        item1.GetComponentInChildren<Text>().text += ": " + player.healItems;
        item2.GetComponentInChildren<Text>().text += ": " +player.defBoostItems;
        item3.GetComponentInChildren<Text>().text += ": " + player.spdBoostItems;
        item4.GetComponentInChildren<Text>().text += ": " + player.atkBoostItems;
        item5.GetComponentInChildren<Text>().text += ": " + player.accBoostItems;
        item6.GetComponentInChildren<Text>().text += ": " + player.critBoostItems;

        enemyHP.text = "HP: "+ enemy.maxHP;
        playerHP.text = "HP: "+ player.maxHP;

        playerImage.sprite = Resources.Load <Sprite> (player.name);
        enemyImage.sprite = Resources.Load <Sprite> (enemy.name);

        playerAnimation = playerImage.GetComponent<Animator>();
        switch (player.name) {
            case "Wet Noodle":
                playerAnimation.Play("wet_noodle");
                break;
            case "Recursive Snail":
                playerAnimation.Play("recursive_snail");
                break;
            case "Joe":
                playerAnimation.Play("joe");
                break;
            case "Gumdrop":
                playerAnimation.Play("gumdrop");
                break;
            case "Firefox":
                playerAnimation.Play("firefox");
                break;
        }
        if(player.name == "Firefox") {
            playerAnimation.transform.Rotate(0, 180, 0);
        }
        enemyAnimation = enemyImage.GetComponent<Animator>();
        switch (enemy.name) {
            case "Wet Noodle":
                enemyAnimation.Play("wet_noodle");
                break;
            case "Recursive Snail":
                enemyAnimation.Play("recursive_snail");
                break;
            case "Joe":
                playerAnimation.Play("joe");
                break;
            case "Gumdrop":
                enemyAnimation.Play("gumdrop");
                break;
            case "Firefox":
                playerAnimation.Play("firefox");
                break;
        }
        if(enemy.name != "Firefox") {
            enemyAnimation.transform.Rotate(0, 180, 0);
        }
        
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
        if(!enemyDecidingMove) {
            displayText.text = "";
            Debug.Log("Our Turn:");
            int totalDmg = 0;
            if(attack < 3) {
                Debug.Log("Basic Attack " + attack);
                displayText.text += "" + player.name + " used " + player.basicAttackNames[attack] + ".\n";
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
                    displayText.text += "" + player.basicAttackNames[attack] + " missed :(\n";
                    Debug.Log("We missed :(");
                }
            } else {
                attack -= 3;
                Debug.Log("Special Attack " + attack);
                displayText.text += "" + player.name + " used " + player.specialAttackNames[attack] + ".\n";
                int newAccuracy = player.specialAttackACC[attack];
                if(accBoost) {
                    newAccuracy = (int)(newAccuracy * 1.1);
                }
                newAccuracy -= enemy.SPD;
                if(UnityEngine.Random.Range(0,101) <= newAccuracy) {
                    totalDmg = (int) (player.specialAttackDMG[attack] * ((double)player.ATK/enemy.DEF));
                    if(Array.IndexOf(player.effectiveTypes[player.TYPE], enemy.TYPE) != -1) {
                        displayText.text += "It's super effective!\n";
                        totalDmg = (int) (totalDmg * player.effectiveMultiplier);
                    }
                    if(Array.IndexOf(player.vulnerableTypes[player.TYPE], enemy.TYPE) != -1) {
                        displayText.text += "It's not very effective.\n";
                        totalDmg = (int) (totalDmg * player.ineffectiveMultiplier);
                    }
                    if(critBoost) {
                        totalDmg = player.applyCrit(totalDmg, true);
                    } else {
                        totalDmg = player.applyCrit(totalDmg, false);
                    }
                } else {
                    displayText.text += "" + player.specialAttackNames[attack] + " missed :(\n";
                    Debug.Log("We missed :(");
                }
            }
        
            Debug.Log("Apply Player Damage: " + totalDmg);
            displayText.text += player.name + " dealt " + totalDmg + " damage!\n";
            enemy.HP -= totalDmg;
            enemy.HP = Math.Max(enemy.HP, 0);
            enemyHP.text = "HP: " + enemy.HP;
            if(enemy.HP == 0) {
                // we killed the enemy with our attack
                // print we won, change scene to wining screen
                Debug.Log("We KOed the Enemy!");
                displayText.text += "We win!";
                StartCoroutine(EndGame());
            } else {
                enemyDecidingMove = true;
                StartCoroutine(EnemyMove());
            }
        }
    }

        
    private IEnumerator EnemyMove() {
        yield return new WaitForSeconds (4);
        displayText.text = "";
        Debug.Log("Enemy Turn");
        int enemyDmg = enemy.getBestMove();
        displayText.text += enemy.name + " dealt " + enemyDmg + " damage!\n";
        enemyHP.text = "HP: " + enemy.HP;
        player.HP -= enemyDmg;
        player.HP = Math.Max(0, player.HP);
        playerHP.text = "HP: " + player.HP;
        if(player.HP == 0) {
            displayText.text += "We lose. :(";
            Debug.Log("Enemy KOed us :(");
            // we died, take us to lose screen
            StartCoroutine(EndGame());
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

        enemyDecidingMove = false;
    }

    private IEnumerator EndGame() {
        yield return new WaitForSeconds (4); 
        GameObject playerGameObj = GameObject.Find("Player");
        Destroy(playerGameObj);
        GameObject enemyGameObj = GameObject.Find("Enemy");
        Destroy(enemyGameObj);
        SceneManager.LoadScene("MainScene");
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
                    item1.GetComponentInChildren<Text>().text = "Morphine: " + player.healItems;
                    Debug.Log("Used heal: HP is now "+ player.HP);
                    displayText.text = "Used heal item!\n";
                    playerHP.text = "HP: "+ player.HP;
                }
                break;
            case 1:
                // DEF boost
                if(player.defBoostItems > 0 && !usedItem) {
                    player.defBoostItems--;
                    player.DEF += 25;
                    item2.GetComponentInChildren<Text>().text = "Kevlar Vest: " + player.defBoostItems;
                    Debug.Log("Used DEF boost: DEF is now " + player.DEF);
                    displayText.text = "Used DEF boost item!\n";
                }
                break;
            case 2:
                // SPD boost
                if(player.spdBoostItems > 0 && !usedItem) {
                    player.spdBoostItems--;
                    player.SPD += 10;
                    item3.GetComponentInChildren<Text>().text = "Redbull: " + player.spdBoostItems;
                    displayText.text = "Used SPD item!\n";
                    Debug.Log("Used SPD boost: SPD is now " + player.SPD);
                }
                break;
            case 3:
                // ATK boost
                if(player.atkBoostItems > 0 && !usedItem) {
                    player.atkBoostItems--;
                    player.ATK += 20;
                    item4.GetComponentInChildren<Text>().text = "Iron Knuckles: " + player.atkBoostItems;
                    displayText.text = "Used ATK boost item!\n";
                    Debug.Log("Used ATK boost: ATK is now "+ player.ATK);
                }
                break;
            case 4:
                // ACC boost
                if(player.accBoostItems > 0 && !usedItem) {
                    player.accBoostItems--;
                    accBoost = true;
                    displayText.text = "Used accuracy item!\n";
                    Debug.Log("Used ACC boost item");
                    item5.GetComponentInChildren<Text>().text = "Glasses: " + player.accBoostItems;
                }
                break;
            case 5:
                // Crit Rate boost
                if(player.critBoostItems > 0 && !usedItem) {
                    player.critBoostItems--;
                    critBoost = true;
                    displayText.text = "Used crit boost item!\n";
                    Debug.Log("Used Crit boost");
                    item6.GetComponentInChildren<Text>().text = "Rabbit Foot: " + player.critBoostItems;
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
