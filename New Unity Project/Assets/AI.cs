using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    public string opponentType;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
