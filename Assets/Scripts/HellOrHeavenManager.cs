using System;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TextMeshProUGUI;
public class HellOrHeavenManager : MonoBehaviour
{
    public Button hellButton;
    public Button heavenButton;

    private SpawnManager spawnManagerScript;

    public int peopleTreated;
    public int peopleInHell;
    public int peopleInHeaven;
    public int missPlacedPeople;

    public NpcProfile currentNpcProfileScript;


    private void Start()
    {
        spawnManagerScript = GetComponent<SpawnManager>();
    }

    public void SendToHell()
    {
        peopleInHell++;
        Destroy(spawnManagerScript.currentNpcIdInMemory);
        spawnManagerScript.BringNewNpc();
    }

    public void SendToParadise()
    {
        peopleInHeaven++;
        Destroy(spawnManagerScript.currentNpcIdInMemory);
        spawnManagerScript.BringNewNpc();
    }

}
