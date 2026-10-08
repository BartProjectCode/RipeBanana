using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.TextMeshProUGUI;

public class GlobalTimerManager : MonoBehaviour
{
    [Header("Shift Timer variables")] public float shiftTimer = 0f;
    public float maxShiftTimerInMinutes = 5f;
    private float maxShiftTimerInSeconds;

    [Header("NPC Timer variables")] public float npcTimer = 0f;
    public float maxNpcTimerInMinutes = 1f;
    public float maxNpcTimerInSeconds;
    public GamePhases currentGamePhase;
    public ComboQualities currentComboQuality;
    public int badComboValue, mediumComboValue, goodComboValue;

    [Header("Phase One NPC max timers in seconds")]
    public float badComboMaxTimerPhaseOne;
    public float mediumComboMaxTimerPhaseOne;
    public float goodComboMaxTimerPhaseOne;

    [Header("Phase Two NPC max timers in seconds")]
    public float badComboMaxTimerPhaseTwo;
    public float mediumComboMaxTimerPhaseTwo;
    public float goodComboMaxTimerPhaseTwo;

    [Header("Phase Three NPC max timers in seconds")]
    public float badComboMaxTimerPhaseThree;
    public float mediumComboMaxTimerPhaseThree;
    public float goodComboMaxTimerPhaseThree;

    [Header("Buttons variables")] public Button startButton;
    public GameObject shiftInteractiblesLayer;

    [HideInInspector] public bool currentlyInShift;

    private SpawnManager spawnManagerScript;
    private HellOrHeavenManager hellOrHeavenManagerScript;

    public TextMeshProUGUI npcCurrentTimerText;
    public TextMeshProUGUI shiftCurrentTimerText;
    public GameObject shiftNotInteractiblePCLayer;

    public GameObject screenStats;

    [Header("End Screen Variables")] public bool showEndScreen;
    public GameObject endScreen;
    public GameObject endScreenVisual;
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

            npcCurrentTimerText.text = "" + Mathf.CeilToInt(npcTimer) + "s";
            shiftCurrentTimerText.text = "" + Mathf.CeilToInt(shiftTimer) + "s";
            
            GamePhaseUpdater();
            ComboQualityUpdater();
            MaxNpcTimerSecondsUpdater();

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
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
            shiftNotInteractiblePCLayer.SetActive(true);
            currentGamePhase = GamePhases.PhaseOne;
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
        endScreenVisual.SetActive(true);
        screenStats.SetActive(false);

        peopleTreatedText.text = "People treated : " + hellOrHeavenManagerScript.peopleTreated;
        correctJudgmentsText.text = "Correct Judgments : " + (hellOrHeavenManagerScript.correctlyPlacedPeopleInHell +
                                                              hellOrHeavenManagerScript.correctlyPlacedPeopleInHeaven);
        correctInHellText.text = "HELL : " + hellOrHeavenManagerScript.correctlyPlacedPeopleInHell;
        correctInHeavenText.text = "HEAVEN : " + hellOrHeavenManagerScript.correctlyPlacedPeopleInHeaven;
        wrongJudgmentsText.text = "Wrong Judgments : " + (hellOrHeavenManagerScript.missPlacedPeopleInHell +
                                                          hellOrHeavenManagerScript.missPlacedPeopleInHeaven);
        wrongInHellText.text = "HELL : " + hellOrHeavenManagerScript.missPlacedPeopleInHell;
        wrongInHeavenText.text = "HEAVEN : " + hellOrHeavenManagerScript.missPlacedPeopleInHeaven;
        peopleTimedOutText.text = "People timed out : " + hellOrHeavenManagerScript.expiredPeople;
        totalPerformancePoints.text = "PERFORMANCE POINTS (PP) : " + hellOrHeavenManagerScript.scoreGlobal;
    }

    public void GamePhaseUpdater()
    {
        if (shiftTimer > (maxShiftTimerInSeconds / 3 * 2))
        {
            currentGamePhase = GamePhases.PhaseOne;
        }
        else if (shiftTimer <= (maxShiftTimerInSeconds / 3 * 2) && shiftTimer > maxShiftTimerInSeconds / 3)
        {
            currentGamePhase = GamePhases.PhaseTwo;
        }
        else if (shiftTimer <= (maxShiftTimerInSeconds / 3))
        {
            currentGamePhase = GamePhases.PhaseThree;
        }
    }

    public void ComboQualityUpdater()
    {
        if (hellOrHeavenManagerScript.multiplier < mediumComboValue)
        {
            currentComboQuality = ComboQualities.BadCombo;
        }
        else if (hellOrHeavenManagerScript.multiplier >= mediumComboValue &&
                 hellOrHeavenManagerScript.multiplier < goodComboValue)
        {
            currentComboQuality = ComboQualities.MediumCombo;
        }
        else if (hellOrHeavenManagerScript.multiplier >= goodComboValue)
        {
            currentComboQuality = ComboQualities.GoodCombo;
        }
    }

    public void MaxNpcTimerSecondsUpdater()
    {
        switch (currentComboQuality)
        {
            case ComboQualities.BadCombo:
                if (currentGamePhase == GamePhases.PhaseOne)
                {
                    maxNpcTimerInSeconds = badComboMaxTimerPhaseOne;
                }
                else if (currentGamePhase == GamePhases.PhaseTwo)
                {
                    maxNpcTimerInSeconds = badComboMaxTimerPhaseTwo;
                }
                else if (currentGamePhase == GamePhases.PhaseThree)
                {
                    maxNpcTimerInSeconds = badComboMaxTimerPhaseThree;
                }

                break;

            case ComboQualities.MediumCombo:
                if (currentGamePhase == GamePhases.PhaseOne)
                {
                    maxNpcTimerInSeconds = mediumComboMaxTimerPhaseOne;
                }
                else if (currentGamePhase == GamePhases.PhaseTwo)
                {
                    maxNpcTimerInSeconds = mediumComboMaxTimerPhaseTwo;
                }
                else if (currentGamePhase == GamePhases.PhaseThree)
                {
                    maxNpcTimerInSeconds = mediumComboMaxTimerPhaseThree;
                }

                break;

            case ComboQualities.GoodCombo:
                if (currentGamePhase == GamePhases.PhaseOne)
                {
                    maxNpcTimerInSeconds = goodComboMaxTimerPhaseOne;
                }
                else if (currentGamePhase == GamePhases.PhaseTwo)
                {
                    maxNpcTimerInSeconds = goodComboMaxTimerPhaseTwo;
                }
                else if (currentGamePhase == GamePhases.PhaseThree)
                {
                    maxNpcTimerInSeconds = goodComboMaxTimerPhaseThree;
                }

                break;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("PrototypeDEV");
    }
}

public enum GamePhases
{
    None,
    PhaseOne,
    PhaseTwo,
    PhaseThree
}

public enum ComboQualities
{
    None,
    BadCombo,
    MediumCombo,
    GoodCombo
}