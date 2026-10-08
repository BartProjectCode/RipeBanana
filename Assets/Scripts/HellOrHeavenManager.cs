using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TextMeshProUGUI;

public class HellOrHeavenManager : MonoBehaviour
{
    public Button hellButton;
    public Button heavenButton;

    public TextMeshProUGUI globalScoreText;

    private SpawnManager spawnManagerScript;

    public NpcProfile currentNpcProfileScript;

    public GlobalTimerManager globalTimerManagerScript;

    [Header("Score")] public int scoreGlobal;

    public int maxMultiplier;

    // public int comboMultiplierDivider;
    public int multiplier;
    public TextMeshProUGUI multiplierText;

    [Header("Statistiques")] public int peopleTreated;
    public int peopleInHell;
    public int peopleInHeaven;
    public int missPlacedPeopleInHell;
    public int correctlyPlacedPeopleInHell;
    public int missPlacedPeopleInHeaven;
    public int correctlyPlacedPeopleInHeaven;
    public int expiredPeople;

    private void Start()
    {
        spawnManagerScript = GetComponent<SpawnManager>();
        globalTimerManagerScript = GetComponent<GlobalTimerManager>();
    }

    public void SendToHell()
    {
        currentNpcProfileScript = spawnManagerScript.npcProfileScript;
        peopleTreated++;
        peopleInHell++;
        // UpdateValuesDependingOnRights(missPlacedPeopleInHell, correctlyPlacedPeopleInHell);

        if (currentNpcProfileScript.rightToHeaven)
        {
            missPlacedPeopleInHell++;
            multiplier = 0;
        }
        else
        {
            correctlyPlacedPeopleInHell++;
            if (multiplier < maxMultiplier)
            {
                multiplier++;
            }

            AddScore(10, multiplier);
        }

        Destroy(spawnManagerScript.currentNpcIdInMemory);
        spawnManagerScript.BringNewNpc();

        globalTimerManagerScript.npcTimer = globalTimerManagerScript.maxNpcTimerInSeconds;
    }

    public void SendToParadise()
    {
        currentNpcProfileScript = spawnManagerScript.npcProfileScript;
        peopleTreated++;
        peopleInHeaven++;
        // UpdateValuesDependingOnRights(correctlyPlacedPeopleInHeaven, missPlacedPeopleInHeaven);
        if (currentNpcProfileScript.rightToHeaven)
        {
            correctlyPlacedPeopleInHeaven++;
            if (multiplier < maxMultiplier)
            {
                multiplier++;
            }

            AddScore(10, multiplier);
            UpdateComboUI();
        }
        else
        {
            missPlacedPeopleInHeaven++;
            multiplier = 0;
            UpdateComboUI();
        }

        Destroy(spawnManagerScript.currentNpcIdInMemory);
        spawnManagerScript.BringNewNpc();

        globalTimerManagerScript.npcTimer = globalTimerManagerScript.maxNpcTimerInSeconds;
    }

    public void UpdateValuesDependingOnRights(int intOne, int intTwo)
    {
        if (currentNpcProfileScript.rightToHeaven)
        {
            intOne++;
        }
        else
        {
            intTwo++;
        }

        globalTimerManagerScript.npcTimer = globalTimerManagerScript.maxNpcTimerInMinutes * 60f;
    }

    public void AddScore(int scoreToAdd, int comboMultiplier)
    {
        scoreGlobal += (scoreToAdd * comboMultiplier);
        globalScoreText.text = " " + scoreGlobal;
    }

    public void UpdateComboUI()
    {
        multiplierText.text = "x" + multiplier;
    }
}