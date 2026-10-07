using System;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform idSpawnPoint;

    [Range(0f, 100f)] public float flawedNpcChance = 0f;

    public List<NpcInfo> npcProfiles;

    public float numberOfFlaws;

    public GameObject npcPrefab;
    private NpcProfile npcProfileScript;

    private GameObject currentNpcInMemory;
    public GameObject currentNpcIdInMemory;
    private HellOrHeavenManager hellOrHeavenManagerScript;

    private void Start()
    {
        hellOrHeavenManagerScript = GetComponent<HellOrHeavenManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BringNewNpc();
        }
    }

    private NpcInfo GenerateRandomNpc()
    {
        return new NpcInfo
        {
            hair = RandomEnum<HairType>(),
            eyes = RandomEnum<EyesType>(),
            nose = RandomEnum<NoseType>(),
            mouth = RandomEnum<MouthType>(),
            skin = RandomEnum<SkinType>(),
            // heaven = true
        };
    }


    public void BringNewNpc()
    {
        if (currentNpcInMemory != null)
        {
            Destroy(currentNpcInMemory);
            Destroy(currentNpcIdInMemory);
        }

        npcProfiles[0] = GenerateRandomNpc();
        npcProfiles[1] = npcProfiles[0];
        float flawedRoll = Random.Range(0f, 100f);

        if (flawedRoll < flawedNpcChance)
        {
            Debug.Log("NPC is FLAWED");
            numberOfFlaws = Mathf.RoundToInt(Random.Range(1, 4));
            Debug.Log("number of flaws = " + numberOfFlaws);

            NpcInfo flawed = npcProfiles[1];
            List<int> features = new List<int> { 0, 1, 2, 3, 4 };

            for (int i = 0; i < numberOfFlaws; i++)
            {
                // pick a feature that hasn't been changed yet
                int pick = Random.Range(0, features.Count);
                int feature = features[pick];
                features.RemoveAt(pick);

                switch (feature)
                {
                    case 0: flawed.hairFlawed = true; break;
                    case 1: flawed.eyesFlawed = true; break;
                    case 2: flawed.noseFlawed = true; break;
                    case 3: flawed.mouthFlawed = true; break;
                    case 4: flawed.skinFlawed = true; break;
                }
            }

            npcProfiles[1] = flawed;

            GameObject currentNpc = Instantiate(npcPrefab, transform.position, transform.rotation);
            npcProfileScript = currentNpc.GetComponent<NpcProfile>();
            npcProfileScript.pcScreenPosition = idSpawnPoint;
            npcProfileScript.thisNpcProfile = npcProfiles[1];
            npcProfileScript.rightToHeaven = false;
            currentNpcInMemory = currentNpc;
            npcProfileScript.SetNpcVisuals();
            currentNpcIdInMemory = npcProfileScript.currentNpcIDProfile;

            // SpawnNpc(1, false);
        }
        else
        {
            Debug.Log("NPC is GOOD");
            GameObject currentNpc = Instantiate(npcPrefab, transform.position, transform.rotation);
            npcProfileScript = currentNpc.GetComponent<NpcProfile>();
            npcProfileScript.pcScreenPosition = idSpawnPoint;
            npcProfileScript.thisNpcProfile = npcProfiles[0];
            npcProfileScript.rightToHeaven = true;
            currentNpcInMemory = currentNpc;
            npcProfileScript.SetNpcVisuals();
            currentNpcIdInMemory = npcProfileScript.currentNpcIDProfile;

            // SpawnNpc(0, true);
        }
    }

    private void SpawnNpc(int npcProfilePos, bool rightToHeavenMethodBool)
    {
        GameObject currentNpc = Instantiate(npcPrefab, transform.position, transform.rotation);
        npcProfileScript = currentNpc.GetComponent<NpcProfile>();
        npcProfileScript.thisNpcProfile = npcProfiles[1];
        npcProfileScript.rightToHeaven = rightToHeavenMethodBool;
        currentNpcInMemory = currentNpc;
        npcProfileScript.SetNpcVisuals();
    }

    private T RandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }

    // private T RandomEnumExcept<T>(T current) where T : Enum
    // {
    //     Array values = Enum.GetValues(typeof(T));
    //     T result;
    //     do
    //     {
    //         result = (T)values.GetValue(Random.Range(0, values.Length));
    //     } while (result.Equals(current));
    //
    //     return result;
    // }

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

[Serializable]
public struct NpcInfo
{
    public HairType hair;
    public EyesType eyes;
    public NoseType nose;
    public MouthType mouth;
    public SkinType skin;
    // public bool heaven;

    public bool hairFlawed;
    public bool eyesFlawed;
    public bool noseFlawed;
    public bool mouthFlawed;
    public bool skinFlawed;
}

[Serializable]
public struct FeatureVariants
{
    public Sprite normal;
    public Sprite[] flawed;
}


public enum HairType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}


public enum EyesType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}

public enum NoseType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}

public enum MouthType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}

public enum SkinType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5
}