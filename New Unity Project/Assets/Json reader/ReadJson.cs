using System.Collections;
using System.IO;  
using System.Collections.Generic;
using UnityEngine;

public class ReadJson : MonoBehaviour
{
    void Start()
    {
        LoadJson();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadJson()
    {
        // TODO: replace name with character selected
        string name = "Squirtle";
        using (StreamReader r = new StreamReader(name + ".json"))
        {
            string json = r.ReadToEnd();
            Character character = JsonUtility.FromJson<Character>(json);
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