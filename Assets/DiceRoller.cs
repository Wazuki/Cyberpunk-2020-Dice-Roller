using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DiceRoller : MonoBehaviour
{

    struct CharacterArmor
    {
        public int headArmorVal; public bool headIsHard;
        public int torsoArmorVal; public bool torsoIsHard;
        public int leftArmArmorVal; public bool leftArmIsHard;
        public int rightArmArmorVal; public bool rightArmIsHard;
        public int leftLegArmorVal; public bool leftLegIsHard;
        public int rightLegArmorVal; public bool rightLegIsHard;
    }

    CharacterArmor targetArmor;
    public RectTransform damagePanel, armorPanel, resultsPanel;
    public Dropdown numDamageDiceDropdown, damageDiceSidesDropdown;
    public Toggle apRoundToggle;
    public Toggle singleShotToggle, threeRoundBurstToggle, fullAutoToggle;
    public InputField rateOfFireInput;
    public InputField hitBonusInput;
    public InputField targetNumberInput;

    public InputField headArmorInputField, torsoArmorInputField, leftArmArmorInputField, rightArmArmorInputField, leftLegArmorInputField, rightLegArmorInputField;
    public Toggle headHardToggle, torsoHardToggle, leftArmHardToggle, rightArmHardToggle, leftLegHardToggle, rightLegHardToggle;

    public Text resultsText;


    void ResetTargetArmorValues()
    {
        targetArmor.headArmorVal = 0; targetArmor.headIsHard = false;
        targetArmor.torsoArmorVal = 0; targetArmor.torsoIsHard = false;
        targetArmor.leftArmArmorVal = 0; targetArmor.leftArmIsHard = false;
        targetArmor.rightArmArmorVal = 0; targetArmor.rightArmIsHard = false;
        targetArmor.leftLegArmorVal = 0; targetArmor.leftLegIsHard = false;
        targetArmor.rightLegArmorVal = 0; targetArmor.rightLegIsHard = false;
    }
    
    void GetTargetArmorValues()
    {
        targetArmor.headArmorVal = System.Int32.Parse(headArmorInputField.text);
        targetArmor.headIsHard = headHardToggle.enabled;
        targetArmor.torsoArmorVal = System.Int32.Parse(torsoArmorInputField.text);
        targetArmor.torsoIsHard = torsoHardToggle.enabled;
        targetArmor.leftArmArmorVal = System.Int32.Parse(leftArmArmorInputField.text);
        targetArmor.leftArmIsHard = leftArmHardToggle.enabled;
        targetArmor.rightArmArmorVal = System.Int32.Parse(rightArmArmorInputField.text);
        targetArmor.rightArmIsHard = rightArmHardToggle.enabled;
        targetArmor.leftLegArmorVal = System.Int32.Parse(leftLegArmorInputField.text);
        targetArmor.leftLegIsHard = leftLegHardToggle.enabled;
        targetArmor.rightLegArmorVal = System.Int32.Parse(rightLegArmorInputField.text);
        targetArmor.rightLegIsHard = rightLegHardToggle.enabled;
    }

    void Awake()
    {

    }

    void Start()
    {
        ResetTargetArmorValues();

        //Intialize number dice and dice sides dropdowns
        List<Dropdown.OptionData> dataToAdd = new List<Dropdown.OptionData>();
        for (int x = 1; x < 11; x++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(x.ToString());
            dataToAdd.Add(option);
        }
        numDamageDiceDropdown.AddOptions(dataToAdd);
        damageDiceSidesDropdown.AddOptions(dataToAdd);

        armorPanel.gameObject.SetActive(false);
        resultsPanel.gameObject.SetActive(false);
    }
    
    public void ToggleArmorPanel()
    {
        armorPanel.gameObject.SetActive(!armorPanel.gameObject.activeSelf);
    }

    public void MakeAttackRolls()
    {
        resultsText.text = "";
        AttackRoll();
        resultsPanel.gameObject.SetActive(true);
    }

    void AttackRoll()
    {
        GetTargetArmorValues();

        int hitBonus = System.Int32.Parse(hitBonusInput.text);
        int targetNum = System.Int32.Parse(targetNumberInput.text);

        //Roll attack
        int hitRoll = 0;
        int attackRoll = 0;
        int rolledValue = 0;

        do
        {
            rolledValue = Random.Range(1, 11);
            //Debug.Log("Rolled " + rolledValue);
            hitRoll += rolledValue;
        } while (rolledValue == 10) ;

        attackRoll = hitRoll + hitBonus;
        //Debug.Log("Rolled (" + hitRoll + "+" + hitBonus + ") " + attackRoll + " vs. TGT " + targetNum);
        if (attackRoll >= targetNum)
        {
            if(fullAutoToggle.isOn)
            {
                int numHits = attackRoll - targetNum;
                int rof = System.Int32.Parse(rateOfFireInput.text);
                if (numHits > rof) numHits = rof;
                Debug.Log("Hit " + numHits + " times with full auto!");
                for(int x = 0; x < numHits; x++)
                {
                    RollDamage();
                }
            }
            else if(threeRoundBurstToggle.isOn)
            {
                int numHits = Random.Range(1, 4);
                Debug.Log("Hit " + numHits + " times with three-round burst!");
                for (int x = 0; x < numHits; x++)
                {
                    RollDamage();
                }
            }
            else
            {
                RollDamage();
            }
        }
        else
        {
            Debug.Log("MISSED!");
            resultsText.text += "MISSED!\n";
        }

    }

    int GetHitLocation()
    {
        int hitLocation = Random.Range(1, 11);
        if (hitLocation == 11) Debug.LogAssertion("HitLocation returned 11! Somethign went wrong!");
        return hitLocation;
    }

    string GetHitLocationName(int hitLoc)
    {
        string hitLocString = "";
        switch (hitLoc)
        {
            case 1:
                hitLocString = "Head";
                break;
            case 2:
            case 3:
            case 4:
                hitLocString = "Torso";
                break;
            case 5:
                hitLocString = "Right Arm";
                break;
            case 6:
                hitLocString = "Left Arm";
                break;
            case 7:
            case 8:
                hitLocString = "Right Leg";
                break;
            case 9:
            case 10:
                hitLocString = "Left Leg";
                break;
            default:
                Debug.LogAssertion("Something went wrong with the hit script!");
                hitLocString = "ERROR!";
                break;
        }
        return hitLocString;
    }

    int GetArmorAtHitLocation(int hitLoc, ref bool armorLocHard)
    {
        switch(hitLoc)
        {
            case 1:
                Debug.Log("Hit head!");
                armorLocHard = targetArmor.headIsHard;
                return targetArmor.headArmorVal;
            case 2:
            case 3:
            case 4:
                Debug.Log("Hit torso!");
                armorLocHard = targetArmor.torsoIsHard;
                return targetArmor.torsoArmorVal;
            case 5:
                Debug.Log("Hit right arm!");
                armorLocHard = targetArmor.rightArmIsHard;
                return targetArmor.rightArmArmorVal;
            case 6:
                Debug.Log("Hit left arm!");
                armorLocHard = targetArmor.leftArmIsHard;
                return targetArmor.leftArmArmorVal;
            case 7:
            case 8:
                Debug.Log("Hit right leg!");
                armorLocHard = targetArmor.rightLegIsHard;
                return targetArmor.rightLegArmorVal;
            case 9:
            case 10:
                Debug.Log("Hit left leg!");
                armorLocHard = targetArmor.leftLegIsHard;
                return targetArmor.leftLegArmorVal;
            default:
                Debug.LogAssertion("Something went wrong with the hit script!");
                return 0;

        }
    }


    void RollDamage()
    {
        int numDamageDice = System.Int32.Parse(numDamageDiceDropdown.value.ToString()) + 1; //+1 to compensate for odd DD issues
        int damageDiceSides = System.Int32.Parse(damageDiceSidesDropdown.value.ToString()) + 1; //+1 to compensate for weird DD issues

        //Debug.LogWarning("Dealing " + numDamageDice + "D" + damageDiceSides);
        int dmgRolled = Random.Range(numDamageDice, (numDamageDice * damageDiceSides) + 1);
        int hitLoc = GetHitLocation();
        bool armorLocHard = false;
        int armorAtLocation = GetArmorAtHitLocation(hitLoc, ref armorLocHard);

        int damageDealt = 0;
        if(apRoundToggle.isOn)
        {
            damageDealt = (dmgRolled - (armorAtLocation / 2)) / 2;
        }
        else
        {
            damageDealt = dmgRolled - armorAtLocation;
        }
        if (damageDealt <= 0) damageDealt = 0;
        if (hitLoc == 1) damageDealt *= 2; //Double damage if head is hit

        //Debug.LogWarning("Rolled " + dmgRolled + " vs. armor " + armorAtLocation + " for " + damageDealt + " damage");
        Debug.Log("Dealt " + damageDealt + " to location " + GetHitLocationName(hitLoc));
        resultsText.text += damageDealt.ToString() + " Damage - " + GetHitLocationName(hitLoc) + "\n";
    }

    public void ReturnButton()
    {
        resultsPanel.gameObject.SetActive(false);
        armorPanel.gameObject.SetActive(false);
    }

}