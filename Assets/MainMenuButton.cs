using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour {

    [SerializeField] GameObject objectToShow;
    [SerializeField] GameObject objectToHide;

    public void MainMenuButton_OnClickHandler()
    {
        objectToShow.SetActive(!objectToShow.activeSelf);
        objectToHide.SetActive(!objectToHide.activeSelf);
    }
}
