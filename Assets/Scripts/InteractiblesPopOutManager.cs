using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractiblesPopOutManager : MonoBehaviour
{
    public GameObject interactiblesLayer;
    public GameObject idCardObject;
    public bool lookingAtPaper;

    public void ShowIDCard()
    {
        interactiblesLayer.SetActive(false);
        idCardObject.SetActive(true);
        lookingAtPaper = true;
    }

    private void Update()
    {
        bool mouseClicked = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if (mouseClicked && lookingAtPaper)
        {
            interactiblesLayer.SetActive(true);
            idCardObject.SetActive(false);
            lookingAtPaper = false;
        }
    }
}
