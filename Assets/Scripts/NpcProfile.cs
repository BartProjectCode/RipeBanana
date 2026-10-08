using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcProfile : MonoBehaviour
{
    public NpcInfo thisNpcProfile;
    public NpcInfo npcIDCard;
    public bool rightToHeaven = true;

    public GameObject idCardObject;
    public GameObject currentNpcIDProfile;

    public Transform pcScreenPosition;

    [SerializedDictionary("HairType", "Sprite")]
    public SerializedDictionary<HairType, FeatureVariants>
        hairTypeSprites = new SerializedDictionary<HairType, FeatureVariants>();

    [SerializedDictionary("EyesType", "Sprite")]
    public SerializedDictionary<EyesType, FeatureVariants>
        eyesTypeSprites = new SerializedDictionary<EyesType, FeatureVariants>();

    [SerializedDictionary("NoseType", "Sprite")]
    public SerializedDictionary<NoseType, FeatureVariants>
        noseTypeSprites = new SerializedDictionary<NoseType, FeatureVariants>();

    [SerializedDictionary("MouthType", "Sprite")]
    public SerializedDictionary<MouthType, FeatureVariants> mouthTypeSprites =
        new SerializedDictionary<MouthType, FeatureVariants>();

    [SerializedDictionary("SkinType", "Sprite")]
    public SerializedDictionary<SkinType, FeatureVariants>
        skinTypeSprites = new SerializedDictionary<SkinType, FeatureVariants>();


    private void Start()
    {
    }

    public void SetNpcVisuals()
    {
        // GameObject hairPrefab = hairTypeSprites[thisNpcProfile.hair];
        // Instantiate(hairPrefab, transform);

        SpawnPart(hairTypeSprites[thisNpcProfile.hair],   thisNpcProfile.hairFlawed, 4);
        SpawnPart(eyesTypeSprites[thisNpcProfile.eyes], thisNpcProfile.eyesFlawed, 3);
        SpawnPart(noseTypeSprites[thisNpcProfile.nose],   thisNpcProfile.noseFlawed, 2);
        SpawnPart(mouthTypeSprites[thisNpcProfile.mouth], thisNpcProfile.mouthFlawed, 1);
        SpawnPart(skinTypeSprites[thisNpcProfile.skin], thisNpcProfile.skinFlawed, 0);
        npcIDCard = thisNpcProfile;
        InstantiateIDCard();
    }

    public void InstantiateIDCard()
    {
        currentNpcIDProfile = Instantiate(idCardObject, pcScreenPosition.transform.position, pcScreenPosition.rotation);
        currentNpcIDProfile.transform.localPosition = new Vector3(-1.51f, -0.1f, 0);
        currentNpcIDProfile.transform.SetParent(pcScreenPosition, false);
        currentNpcIDProfile.GetComponent<NpcIDProfile>().npcProfileScript = this;
        currentNpcIDProfile.GetComponent<NpcIDProfile>().SetIDVisuals();
        // currentNpcIDProfile.gameObject.SetActive(false);
    }

    private void SpawnPart(FeatureVariants variants, bool isFlawed, int order)
    {
        Sprite sprite = variants.normal;

        if (isFlawed && variants.flawed.Length > 0)
        {
            sprite = variants.flawed[Random.Range(0, variants.flawed.Length)];
        }

        GameObject part = new GameObject(sprite.name); // new empty object
        part.transform.SetParent(transform, false); // child of the NPC, at its position

        SpriteRenderer sr = part.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
    }

    private void OnDestroy()
    {
        // Destroy(idCardObject);
    }


    // [SerializedDictionary("HairType", "Sprite")]
    // public SerializedDictionary<HairType1Flawed, GameObject> hairTypeSpritesFlawed = new SerializedDictionary<HairType1Flawed, GameObject>();
}