using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class SaveData
{
    public int coins;
    public int diamonds;
    public int countryBest, iceBest, moonBest, rockyBest;
    public int[] engineLevel, suspensionLevel, tyreLevel;
    public bool[] vehicleBought,levelBought;
    public SaveData()
    {
        coins = CollectibleControl.coinCount;
        diamonds = CollectibleControl.diamondCount;
        countryBest = UIScript.countryBest;
        iceBest = UIScript.iceBest;
        moonBest = UIScript.moonBest;
        rockyBest = UIScript.rockyBest;
        engineLevel = UIScript.engineLevel;
        suspensionLevel = UIScript.suspensionLevel;
        tyreLevel = UIScript.tyreLevel;
        vehicleBought = UIScript.vehicleBought;
        levelBought = UIScript.levelBought;
    }
}
