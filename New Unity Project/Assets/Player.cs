using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    Dictionary<string, int> statsMap = new Dictionary<string, int>();
    Dictionary<string, Dictionary<string, List<int>>> attacksMap = new Dictionary<string, Dictionary<string, List<int>>>();

    [DllImport("Player", EntryPoint = "read_stats")]
    public static extern void read_stats(List<string> statKeys, List<int> statValues, List<string> attackKeys,
		List<int> attackDmg, List<int> attackAccuracy, string fileName);

    // Start is called before the first frame update
    void Start()
    {
        List<string> statKeys = new List<string>();
        List<int> statValues = new List<int>();
        List<string> attackKeys = new List<string>();
		List<int> attackDmg = new List<int>(); 
        List<int> attackAccuracy = new List<int>();
        
        read_stats(statKeys, statValues, attackKeys, attackDmg, attackAccuracy, "Squirtle.txt");

        //fill statsMap
        for(int i = 0; i < statKeys.Count; i++) {
            statsMap.Add(statKeys[i], statValues[i]);
        }

        //fill attacksMap, first 3 attacks are always basic and next 3 are special
        attacksMap.Add("BasicAttack", new Dictionary<string, List<int>>());
        attacksMap.Add("SpecialAttack", new Dictionary<string, List<int>>());
        for(int i = 0; i < attackKeys.Count; i++) {
            List<int> attackVals = new List<int>();
            attackVals.Add(attackDmg[i]);
            attackVals.Add(attackAccuracy[i]);
            if(i < 3) {
                attacksMap["BasicAttack"].Add(attackKeys[i], attackVals);
            } else {
                attacksMap["SpecialAttack"].Add(attackKeys[i], attackVals);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
