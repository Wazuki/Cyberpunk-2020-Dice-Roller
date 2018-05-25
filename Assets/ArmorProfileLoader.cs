using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorProfileLoader : MonoBehaviour
{
    const int ARMOR_PROFILE_OFFSET = 1;
    const int NEW_PROFILE_INDEX = 0;
    const string newProfileString = "New Profile";

    [SerializeField] ArmorProfileManager profileManager;
    [SerializeField] Dropdown armorProfileDropdown;
    [SerializeField] DiceRoller diceRoller;

    private void OnEnable()
    {
        ResetArmorProfileDropdown();
    }

    public void OnArmorProfileSelect()
    {
        //Load the armor profile if "New Profile" is not selected
        if(armorProfileDropdown.captionText.text == newProfileString)
        {
            Debug.Log("New profile selected!");
            diceRoller.ClearArmorAction();
        }
        else
        {
            Debug.Log("Selected profile " + armorProfileDropdown.captionText.text + " - should be index " +
                (armorProfileDropdown.value - ARMOR_PROFILE_OFFSET));
            diceRoller.SelectArmorProfle(profileManager.armorProfileList[armorProfileDropdown.value - ARMOR_PROFILE_OFFSET]);
        }
    }

    public void SelectNewProfle()
    {
        armorProfileDropdown.value = 0;
        armorProfileDropdown.Select();
        armorProfileDropdown.RefreshShownValue();
    }

    public void ResetArmorProfileDropdown()
    {
        //Load the armor profiles from teh profile manager
        armorProfileDropdown.ClearOptions();

        List<string> armorStringNames = new List<string> { newProfileString };

        for (int x = 0; x < profileManager.armorProfileList.Count; x++)
        {
            armorStringNames.Add(profileManager.armorProfileList[x].profileName);
        }

        armorProfileDropdown.AddOptions(armorStringNames);

        SelectNewProfle();
    }
}
