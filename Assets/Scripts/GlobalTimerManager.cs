using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TextMeshProUGUI;

public class GlobalTimerManager : MonoBehaviour
{
    [Header("Shift Timer variables")] public float shiftTimer = 0f;
    public float maxShiftTimerInMinutes = 5f;
    private float maxShiftTimerInSeconds;

    [Header("NPC Timer variables")] public float npcTimer = 0f;
    public float maxNpcTimerInMinutes = 1f;
    private float maxNpcTimerInSeconds;

    [Header("Buttons variables")] public Button startButton;
    public GameObject shiftInteractiblesLayer;

    [HideInInspector] public bool currentlyInShift;

    private SpawnManager spawnManagerScript;
    private HellOrHeavenManager hellOrHeavenManagerScript;

    public bool showEndScreen;

    public GameObject endScreen;

    public TextMeshProUGUI peopleTreatedText;
    public TextMeshProUGUI correctJudgmentsText;
    public TextMeshProUGUI correctInHellText;
    public TextMeshProUGUI correctInHeavenText;
    public TextMeshProUGUI wrongJudgmentsText;
    public TextMeshProUGUI wrongInHellText;
    public TextMeshProUGUI wrongInHeavenText;
    public TextMeshProUGUI peopleTimedOutText;
    public TextMeshProUGUI totalPerformancePoints;

    void Start()
    {
        maxShiftTimerInSeconds = maxShiftTimerInMinutes * 60;
        shiftTimer = maxShiftTimerInSeconds;

        maxNpcTimerInSeconds = maxNpcTimerInMinutes * 60;
        npcTimer = maxNpcTimerInSeconds;

        startButton.onClick.AddListener(StartShift);

        spawnManagerScript = GetComponent<SpawnManager>();
        hellOrHeavenManagerScript = GetComponent<HellOrHeavenManager>();
    }

    void Update()
    {
        if (currentlyInShift)
        {
            ShiftCountDownEachFrame();
            NPCCountDownEachFrame();
        }
    }


    public void StartShift()
    {
        if (!currentlyInShift)
        {
            currentlyInShift = true;
            spawnManagerScript.BringNewNpc();
            startButton.gameObject.SetActive(false);
            shiftInteractiblesLayer.SetActive(true);
        }
    }

    public void ShiftCountDownEachFrame()
    {
        if (shiftTimer > 0)
        {
            shiftTimer -= Time.deltaTime;
            // Debug.Log("Global Timer = " + shiftTimer);
        }
        else
        {
            currentlyInShift = false;
            shiftTimer = 0;
            EndTimeLogic();
            // Debug.Log("Shift OVER !");
        }
    }

    private void NPCCountDownEachFrame()
    {
        if (npcTimer > 0)
        {
            npcTimer -= Time.deltaTime;
        }
        else
        {
            npcTimer = maxNpcTimerInSeconds;
            hellOrHeavenManagerScript.expiredPeople++;
            spawnManagerScript.BringNewNpc();
        }
    }

    public void EndTimeLogic()
    {
        shiftInteractiblesLayer.gameObject.SetActive(false);
        endScreen.SetActive(true);

        peopleTreatedText.text = "People treated : " + hellOrHeavenManagerScript.peopleTreated;
        correctJudgmentsText.text = "Correct Judgments : " + (hellOrHeavenManagerScript.correctlyPlacedPeopleInHell + hellOrHeavenManagerScript.correctlyPlacedPeopleInHeaven);
        correctInHellText.text = "HELL : " + hellOrHeavenManagerScript.correctlyPlacedPeopleInHell;
        correctInHeavenText.text = "HEAVEN : " + hellOrHeavenManagerScript.correctlyPlacedPeopleInHeaven;
        wrongJudgmentsText.text = "Wrong Judgments : " + (hellOrHeavenManagerScript.missPlacedPeopleInHell + hellOrHeavenManagerScript.missPlacedPeopleInHeaven);
        wrongInHellText.text = "HELL : " + hellOrHeavenManagerScript.missPlacedPeopleInHell;
        wrongInHeavenText.text = "HEAVEN : " + hellOrHeavenManagerScript.missPlacedPeopleInHeaven;
        peopleTimedOutText.text = "People timed out : " + hellOrHeavenManagerScript.expiredPeople;
        totalPerformancePoints.text = "PERFORMANCE POINTS (PP) : " + hellOrHeavenManagerScript.scoreGlobal;
    }
}