using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArmorReference : MonoBehaviour
{
    public CharacterArmor profile;
    Button button;
    public bool selected = false;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite normalSprite;

    // Use this for initialization
    public void AssignProfile(CharacterArmor characterArmor)
    {
        profile = characterArmor;

        button = GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = profile.profileName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickHandler);
    }
    
	public void OnClickHandler()
    {
        ColorBlock block = button.colors;

        if (!selected)
        {
            //block.normalColor = Color.blue;
            //button.colors = block;
            button.image.overrideSprite = selectedSprite;

            selected = true;
        }
        else if(selected)
        {
            //block.normalColor = Color.white;
            //button.colors = block;
            button.image.overrideSprite = normalSprite;

            selected = false;
        }
    }

    public void ResetStatus()
    {
        button.image.overrideSprite = normalSprite;
        selected = false;
    }
}
