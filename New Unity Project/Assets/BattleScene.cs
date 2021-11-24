using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleScene : MonoBehaviour
{

    public Button basicAttack1, basicAttack2, basicAttack3, specialAttack1, specialAttack2, specialAttack3;
    public Text playerHP;
    public Text enemyHP;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerGameObj = GameObject.Find("Player");
        if (playerGameObj != null) {
            player = playerGameObj.GetComponent<Player>();
            Debug.Log("Player name is: " + player.name);
        } else {
            Debug.Log("Player game obj was null");
        }
        
        basicAttack1.onClick.AddListener(delegate { executeAttack(1); });
        basicAttack2.onClick.AddListener(delegate { executeAttack(2); });
        basicAttack3.onClick.AddListener(delegate { executeAttack(3); });
        specialAttack1.onClick.AddListener(delegate { executeAttack(4); });
        specialAttack2.onClick.AddListener(delegate { executeAttack(5); });
        specialAttack3.onClick.AddListener(delegate { executeAttack(6); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void executeAttack(int attack) {
        switch(attack) {
            case 1:
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
    
}
