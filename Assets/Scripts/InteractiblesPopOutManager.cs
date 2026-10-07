using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractiblesPopOutManager : MonoBehaviour
{
    public GameObject interactiblesLayer;
    public GameObject idCardObject;
    public GameObject scrollObject;
    public GameObject deathSkullObject;

    public UITypes uiCurrentlyLookedAt;

    public void ShowIDCard()
    {
        interactiblesLayer.SetActive(false);
        idCardObject.SetActive(true);
        uiCurrentlyLookedAt = UITypes.IDCard;
    }

    public void ShowScroll()
    {
        interactiblesLayer.SetActive(false);
        scrollObject.SetActive(true);
        uiCurrentlyLookedAt = UITypes.Scroll;
    }

    public void ShowDeathSkull()
    {
        interactiblesLayer.SetActive(false);
        deathSkullObject.SetActive(true);
        uiCurrentlyLookedAt = UITypes.DeathSkull;
    }

    private void Update()
    {
        bool mouseOrEscapeClicked = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ||
                                    Input.GetKeyDown(KeyCode.Escape);

        bool currentlyLookingAtUI = (uiCurrentlyLookedAt == UITypes.IDCard) ||
                                    (uiCurrentlyLookedAt == UITypes.Scroll) ||
                                    (uiCurrentlyLookedAt == UITypes.DeathSkull);

        if (mouseOrEscapeClicked && currentlyLookingAtUI)
        {
            interactiblesLayer.SetActive(true);
            idCardObject.SetActive(false);
            scrollObject.SetActive(false);
            deathSkullObject.SetActive(false);
            uiCurrentlyLookedAt = UITypes.None;
        }
    }
}

public enum UITypes
{
    None,
    IDCard,
    Scroll,
    DeathSkull
}