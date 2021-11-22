using System.Collections;
using System.IO;  
using System.Collections.Generic;
using UnityEngine;

public class ReadJson
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Character LoadJson(string name)
    {
        using (StreamReader r = new StreamReader("Assets/Characters/" + name + ".json"))
        {
            string json = r.ReadToEnd();
            Character character = JsonUtility.FromJson<Character>(json);

            return character;
        }
    }

    [System.Serializable]
    public class Character
    {
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
    }
}