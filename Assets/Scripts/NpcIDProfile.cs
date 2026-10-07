using UnityEngine;

public class NpcIDProfile : MonoBehaviour
{
    public NpcInfo thisNpcProfile;
    public NpcProfile npcProfileScript;
    
    public void SetIDVisuals()
    {
        NpcInfo info = npcProfileScript.npcIDCard;
        Debug.Log("ID card eyes = " + info.eyes + ", skin = " + info.skin);
        
        // GameObject hairPrefab = hairTypeSprites[thisNpcProfile.hair];
        // Instantiate(hairPrefab, transform);
        
        // SpawnIDCardPart(npcProfileScript.hairTypeSprites[info.hair], 4);
        SpawnIDCardPart(npcProfileScript.eyesTypeSprites[info.eyes],3);
        SpawnIDCardPart(npcProfileScript.noseTypeSprites[info.nose], 2);
        SpawnIDCardPart(npcProfileScript.mouthTypeSprites[info.mouth], 1);
        SpawnIDCardPart(npcProfileScript.skinTypeSprites[info.skin], 0);
        
    }
    
    private void SpawnIDCardPart(FeatureVariants variants, int order)
    {
        Sprite sprite = variants.normal;

        GameObject part = new GameObject(sprite.name);
        part.transform.SetParent(transform, false);

        SpriteRenderer sr = part.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
    }
}
