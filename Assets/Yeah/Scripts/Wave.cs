using System;
using UnityEngine;

[Serializable]
public class Wave : MonoBehaviour
{
    [SerializeField] public DictionaryItem[] dictionaryItems;

    public int Spawn(Vector3 spawnPosition)
    {
        int enemiesSpawned = 0;

        foreach (var item in dictionaryItems)
        {
            for (int i = 0; i < item.count; i++)
            {
                Instantiate(item.enemy, spawnPosition + new Vector3(i * 2, 0 , i * 2), Quaternion.identity);
                enemiesSpawned++;
            }
        }
        return enemiesSpawned;
    }
}

[Serializable]
public class DictionaryItem
{
    public Enemy enemy;
    public int count;
}