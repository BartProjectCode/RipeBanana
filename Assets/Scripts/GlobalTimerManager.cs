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
            shiftTimer = 0;
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
            //Fire current NPC and spawn a new NPC.
        }
    }

    public void EndTimeLogic()
    {
        //Execute every method that needs to be played when the shift is over.
    }
}