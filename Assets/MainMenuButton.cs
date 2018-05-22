using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour {

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject targetObject;

    public void MainMenuButton_OnClickHandler()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
