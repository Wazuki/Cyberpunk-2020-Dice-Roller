using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ArmorProfileManager : MonoBehaviour
{

    public List<CharacterArmor> armorProfileList;
    [SerializeField] RectTransform profileManagerContent;
    [SerializeField] GameObject profileManagerPrefabButton;

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
        ReloadList();
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

    public void InitializeProfileManger()
    {
        foreach (Transform c in profileManagerContent.transform) Destroy(c.gameObject);

        for(int x = 0; x < armorProfileList.Count; x++)
        {
            //Instantiate a copy of the prefab button manager, call its initializer with the current profile.
            GameObject newButton = Instantiate(profileManagerPrefabButton, profileManagerContent);
            newButton.GetComponent<CharacterArmorReference>().AssignProfile(armorProfileList[x]);
        }
    }

    public void DeleteProfiles()
    {
        //Confirm that profiles are going to be deleted.
        ModalPanel modalPanel = ModalPanel.Instance();
        modalPanel.Choice("Delete profiles?", ConfirmProfileDeletion, AbortAction);
    }

    public void ConfirmProfileDeletion()
    {
        for (int x = profileManagerContent.childCount - 1; x >= 0; x--)
        {
            CharacterArmorReference characterArmorRef = profileManagerContent.GetChild(x).GetComponent<CharacterArmorReference>();

            if (characterArmorRef.selected)
            {
                armorProfileList.Remove(characterArmorRef.profile);

            }

            Destroy(profileManagerContent.GetChild(x).gameObject);
        }

        string armorProfileJSON = JsonHelper.ToJson(armorProfileList, true);
        File.WriteAllText(filePath, armorProfileJSON);

        ReloadList();
        InitializeProfileManger();
    }

    private void ReloadList()
    {
        armorProfileList.Clear();
        LoadArmorProfles();
    }
}
