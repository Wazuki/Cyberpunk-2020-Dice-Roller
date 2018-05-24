using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ArmorProfileManager : MonoBehaviour
{
    [SerializeField] public List<CharacterArmor> armorProfileList;
    [SerializeField]

    public InputField profileNameInputField;

    public List<InputField> armorInputFields;
    public List<Toggle> armorhardnessToggles;

    CharacterArmor newArmorprofile = new CharacterArmor();

    string filePath;
    string armorDataFileName = "armor_profiles.json";

    public Button saveButton;

    private void Start()
    {
        armorProfileList = new List<CharacterArmor>();
        newArmorprofile.Initialize();

        filePath = Path.Combine(Application.persistentDataPath, armorDataFileName);

        LoadArmorProfles();
    }

    public void SaveArmorProfile()
    {


        //Check for blank strings before continuing
        for(int x = 0; x < CharacterArmor.ARMOR_LOCATIONS; x++)
        {
            if (armorInputFields[x].text == "")
            {
                ModalPanel modalPanel = ModalPanel.Instance();

                modalPanel.Choice("One of the armor fields is blank! This will be marked as 0 armor! Continue?", FinishSaveProfile, AbortAction);
                return;
            }

        }

        FinishSaveProfile();
    }

    public void FinishSaveProfile()
    {
        Debug.Log("Finishing save!");
        newArmorprofile.ResetArmorValues();

        for(int x = 0; x < CharacterArmor.ARMOR_LOCATIONS; x++)
        {
            newArmorprofile.AssignArmorValue(armorInputFields[x].text, x);
            newArmorprofile.isHard[x] = armorhardnessToggles[x].isOn;
        }

        newArmorprofile.profileName = profileNameInputField.text;

        armorProfileList.Add(newArmorprofile);

        //Serialize this to JSON.
        //string armorProfileJSON = JsonUtility.ToJson(armorProfileList, true);
        string armorProfileJSON = JsonHelper.ToJson(armorProfileList, true);
        File.WriteAllText(filePath, armorProfileJSON);

    }

    public void AbortAction()
    {
        Debug.Log("Aborting action!");
    }

    public void LoadArmorProfles()
    {
        if(File.Exists(filePath))
        {
            string armorProfileJSON = File.ReadAllText(filePath);
            armorProfileList = JsonHelper.FromJson<CharacterArmor>(armorProfileJSON);

            Debug.Log("Profile Count: " + armorProfileList.Count);
        }
        else
        {
            Debug.Log("No data to load!");
        }
    }

    public void EmptyNameChecker()
    {
        if (profileNameInputField.text != "") saveButton.interactable = true;
        else saveButton.interactable = false;
    }

    public void ClearDataModal()
    {
        ModalPanel modalPanel = ModalPanel.Instance();

        modalPanel.Choice("Clear data?", ClearData, AbortAction);
    }

    public void ClearData()
    {
        for(int x = 0; x < CharacterArmor.ARMOR_LOCATIONS; x++)
        {
            armorInputFields[x].text = "";
            armorhardnessToggles[x].isOn = false;
        }

        profileNameInputField.text = "";
        saveButton.interactable = false;
    }

}
