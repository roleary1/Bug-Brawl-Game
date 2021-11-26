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
    public Text playerHP;
    public Text enemyHP;

    public Player player;

    public AI enemy;

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

        
        basicAttack1.onClick.AddListener(delegate { executeAttack(1); });
        basicAttack2.onClick.AddListener(delegate { executeAttack(2); });
        basicAttack3.onClick.AddListener(delegate { executeAttack(3); });
        specialAttack1.onClick.AddListener(delegate { executeAttack(4); });
        specialAttack2.onClick.AddListener(delegate { executeAttack(5); });
        specialAttack3.onClick.AddListener(delegate { executeAttack(6); });


        attackWindowButton.onClick.AddListener(delegate { changeTab("attack"); });
        itemWindowButton.onClick.AddListener(delegate { changeTab("items"); });

        item1.onClick.AddListener(delegate { useItem(1); });
        item2.onClick.AddListener(delegate { useItem(2); });
        item3.onClick.AddListener(delegate { useItem(3); });
        item4.onClick.AddListener(delegate { useItem(4); });
        item5.onClick.AddListener(delegate { useItem(5); });
        item6.onClick.AddListener(delegate { useItem(6); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void executeAttack(int attack) {
        switch(attack) {
            case 1:
                Debug.Log("Basic Attack 1");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    void useItem(int item) {
        switch(item) {
            case 1:
                Debug.Log("Item 1");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
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
