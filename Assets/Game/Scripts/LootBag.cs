using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LootBag : MonoBehaviour
{
    public LootCollectable droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();
    float maxDropDistance = 1.5f;

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();

        foreach(Loot item in lootList)
        {
            if(randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0)
        {
                Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
                return droppedItem;
        }
        Debug.Log("No item dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition, int itemCount)
    {
        for(int i = 0; i < itemCount; i++)
        {
            Loot droppedItem = GetDroppedItem();
            if(droppedItem != null)
            {
                Vector3 randomOffset = Random.insideUnitSphere * maxDropDistance;
                randomOffset.y = 0;
                Vector3 spawnPositionWithOffset = spawnPosition + randomOffset;

                LootCollectable lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.initCollectable(droppedItem.lootName);
                lootGameObject.GetComponentInChildren<SpriteRenderer>().sprite = droppedItem.lootSprite;
                lootGameObject.transform.DOJump(spawnPositionWithOffset, 2.5f, 1, .5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    lootGameObject.isCollectable = true;
                });
            }
        }
    }
}