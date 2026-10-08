using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractiblesPopOutManager : MonoBehaviour
{
    public GameObject interactiblesLayer;
    public GameObject idCardObject;
    public GameObject scrollObject;
    public GameObject scrollVisual;
    public GameObject deathSkullObject;
    public GameObject deathSkullVisual;

    public GameObject screenStats;

    public UITypes uiCurrentlyLookedAt;

    public void ShowIDCard()
    {
        interactiblesLayer.SetActive(false);
        idCardObject.SetActive(true);
        HideStats();
        uiCurrentlyLookedAt = UITypes.IDCard;
    }

    public void ShowScroll()
    {
        interactiblesLayer.SetActive(false);
        scrollObject.SetActive(true);
        scrollVisual.SetActive(true);
        HideStats();
        uiCurrentlyLookedAt = UITypes.Scroll;
    }

    public void ShowDeathSkull()
    {
        interactiblesLayer.SetActive(false);
        deathSkullObject.SetActive(true);
        deathSkullVisual.SetActive(true);
        HideStats();
        uiCurrentlyLookedAt = UITypes.DeathSkull;
    }

    public void ShowStats()
    {
        screenStats.SetActive(true);
    }
    public void HideStats()
    {
        screenStats.SetActive(false);
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
            scrollVisual.SetActive(false);
            deathSkullObject.SetActive(false);
            deathSkullVisual.SetActive(false);
            ShowStats();
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