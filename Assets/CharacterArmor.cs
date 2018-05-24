using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterArmor
{
    public string profileName;
    public const int ARMOR_LOCATIONS = 6;

    public int[] armorVal;
    public bool[] isHard;

    public void Initialize()
    {
        //armorVal = new List<int>(ARMOR_LOCATIONS);
        //isHard = new List<bool>(ARMOR_LOCATIONS);
        armorVal = new int[ARMOR_LOCATIONS];
        isHard = new bool[ARMOR_LOCATIONS];

        ResetArmorValues();
    }

    public void ResetArmorValues()
    {
        for (int x = 0; x < ARMOR_LOCATIONS; x++)
        {
            armorVal[x] = 0;
            isHard[x] = false;
        }
    }

    public void AssignArmorValue(string armorValString, int armorLocation)
    {
        if (!System.Int32.TryParse(armorValString, out armorVal[armorLocation])) armorVal[armorLocation] = 0;
    }
}
