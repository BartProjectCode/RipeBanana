using System.Xml.Schema;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [Range(0f, 100f)] public float flawedNpcChance = 0f;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BringNewNpc();
        }
    }

    public void BringNewNpc()
    {
        float flawedRoll = Random.Range(0f, 100f);

        if (flawedRoll < flawedNpcChance)
        {
            Debug.Log("NPC is FLAWED");
        }
        else
        {
            Debug.Log("NPC is GOOD");
        }
    }

    // [ContextMenu("Test Chance")]
    // void TestChance()
    // {
    //     int flawedCount = 0;
    //     int total = 100000;
    //
    //     for (int i = 0; i < total; i++)
    //     {
    //         if (Random.Range(0f, 100f) < flawedNpcChance)
    //             flawedCount++;
    //     }
    //
    //     float actual = (float)flawedCount / total * 100f;
    //     Debug.Log($"Set: {flawedNpcChance}% | Actual: {actual:F2}%");
    // }
}

public struct NpcInfo
{
    // HeadType head =
}


public enum hairType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}