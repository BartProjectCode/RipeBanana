using System;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
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
    public NpcProfile npcProfileScript;

    private GameObject currentNpcInMemory;
    public GameObject currentNpcIdInMemory;
    private HellOrHeavenManager hellOrHeavenManagerScript;

    [Header("Facts")] public string[] goodFacts;
    public string[] badFacts;
    public string[] goodDeaths;
    public string[] badDeaths;

    public TextMeshProUGUI npcFactsUIText1;
    public TextMeshProUGUI npcFactsUIText2;
    public TextMeshProUGUI npcFactsUIText3;
    public TextMeshProUGUI npcDeathUIText;

    public Transform npcSpawnPoint;

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
        string[] facts = PickGoodFacts();

        return new NpcInfo
        {
            hair = RandomEnum<HairType>(),
            eyes = RandomEnum<EyesType>(),
            nose = RandomEnum<NoseType>(),
            mouth = RandomEnum<MouthType>(),
            skin = RandomEnum<SkinType>(),
            fact1 = facts[0],
            fact2 = facts[1],
            fact3 = facts[2],
            death = goodDeaths[Random.Range(0, goodDeaths.Length)]
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
            numberOfFlaws = Random.Range(1, 4); // 1, 2 or 3 flaws in total

            NpcInfo flawed = npcProfiles[1];

// Decide which slots this NPC is allowed to be flawed in
            List<int> slots;
            float typeRoll = Random.value;

            if (typeRoll < 0.3f)
                slots = new List<int> { 5, 6, 7, 8 }; // 30%: paperwork only (facts + death)
            else if (typeRoll < 0.6f)
                slots = new List<int> { 0, 1, 2, 3, 4 }; // 30%: looks only
            else
                slots = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // 40%: anything

            List<string> badFactPool = new List<string>(badFacts);

            for (int i = 0; i < numberOfFlaws; i++)
            {
                int pick = Random.Range(0, slots.Count);
                int slot = slots[pick];
                slots.RemoveAt(pick);

                switch (slot)
                {
                    case 0: flawed.hairFlawed = true; break;
                    case 1: flawed.eyesFlawed = true; break;
                    case 2: flawed.noseFlawed = true; break;
                    case 3: flawed.mouthFlawed = true; break;
                    case 4: flawed.skinFlawed = true; break;

                    case 5:
                    case 6:
                    case 7: // fact slot 0, 1 or 2
                        if (badFactPool.Count > 0)
                        {
                            int badPick = Random.Range(0, badFactPool.Count);
                            flawed.SetFact(slot - 5, badFactPool[badPick]);
                            badFactPool.RemoveAt(badPick);
                        }

                        break;

                    case 8: // cause of death
                        if (badDeaths.Length > 0)
                        {
                            flawed.death = badDeaths[Random.Range(0, badDeaths.Length)];
                        }

                        break;
                }
            }

            npcProfiles[1] = flawed;

            GameObject currentNpc = Instantiate(npcPrefab, transform.position, transform.rotation);
            currentNpc.transform.SetParent(npcSpawnPoint, false);
            npcProfileScript = currentNpc.GetComponent<NpcProfile>();
            npcProfileScript.pcScreenPosition = idSpawnPoint;
            npcProfileScript.thisNpcProfile = npcProfiles[1];
            npcProfileScript.rightToHeaven = false;
            hellOrHeavenManagerScript.currentNpcProfileScript = npcProfileScript;
            currentNpcInMemory = currentNpc;
            npcProfileScript.SetNpcVisuals();
            currentNpcIdInMemory = npcProfileScript.currentNpcIDProfile;
            
            UpdateFactsAndDeath(npcProfileScript.thisNpcProfile.fact1, npcProfileScript.thisNpcProfile.fact2,
                npcProfileScript.thisNpcProfile.fact3, npcProfileScript.thisNpcProfile.death);

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
            hellOrHeavenManagerScript.currentNpcProfileScript = npcProfileScript;
            currentNpcInMemory = currentNpc;
            npcProfileScript.SetNpcVisuals();
            currentNpcIdInMemory = npcProfileScript.currentNpcIDProfile;
            
            UpdateFactsAndDeath(npcProfileScript.thisNpcProfile.fact1, npcProfileScript.thisNpcProfile.fact2,
                npcProfileScript.thisNpcProfile.fact3, npcProfileScript.thisNpcProfile.death);


            // SpawnNpc(0, true);
        }
        currentNpcInMemory.transform.SetParent(npcSpawnPoint, false);
        
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

    private string[] PickGoodFacts()
    {
        List<string> pool = new List<string>(goodFacts); // copy, so the original isn't changed
        string[] result = new string[3];

        for (int i = 0; i < 3; i++)
        {
            int pick = Random.Range(0, pool.Count);
            result[i] = pool[pick];
            pool.RemoveAt(pick); // can't be picked twice
        }

        return result;
    }

    private void UpdateFactsAndDeath(string textOne, string textTwo, string textThree, string deathText)
    {
        npcFactsUIText1.text = textOne;
        npcFactsUIText2.text = textTwo;
        npcFactsUIText3.text = textThree;
        npcDeathUIText.text = deathText;
    }
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

    public string fact1;
    public string fact2;
    public string fact3;
    public string death;

    public void SetFact(int slot, string text)
    {
        if (slot == 0) fact1 = text;
        if (slot == 1) fact2 = text;
        if (slot == 2) fact3 = text;
    }
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