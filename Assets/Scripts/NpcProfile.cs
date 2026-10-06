using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcProfile : MonoBehaviour
{
    public NpcInfo thisNpcProfile;
    public bool RightToHeaven = true;

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
        
        SpawnPart(hairTypeSprites[thisNpcProfile.hair],   thisNpcProfile.hairFlawed);
        
        // GameObject eyesPrefab = eyesTypeSprites[thisNpcProfile.eyes];
        // Instantiate(eyesPrefab, transform);
        //
        // GameObject nosePrefab = noseTypeSprites[thisNpcProfile.nose];
        // Instantiate(nosePrefab, transform);
        //
        // GameObject mouthPrefab = mouthTypeSprites[thisNpcProfile.mouth];
        // Instantiate(mouthPrefab, transform);
        //
        // GameObject skinPrefab = skinTypeSprites[thisNpcProfile.skin];
        // Instantiate(skinPrefab, transform);
    }
    
    private void SpawnPart(FeatureVariants variants, bool isFlawed)
    {
        GameObject prefab = variants.normal;

        if (isFlawed && variants.flawed.Length > 0)
        {
            // pick one of the flawed versions at random
            prefab = variants.flawed[Random.Range(0, variants.flawed.Length)];
        }

        Instantiate(prefab, transform);
    }


    // [SerializedDictionary("HairType", "Sprite")]
    // public SerializedDictionary<HairType1Flawed, GameObject> hairTypeSpritesFlawed = new SerializedDictionary<HairType1Flawed, GameObject>();
}