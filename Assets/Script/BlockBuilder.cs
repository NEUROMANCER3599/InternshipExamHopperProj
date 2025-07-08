using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("Block Spawn Parameters")]
    [SerializeField] private List<Entity> BlockList;
    public Vector3 StartingBlockPosition = new Vector3(0, -1.5f, 0);
    [SerializeField] private Entity StartingBlockPrefab;
    [SerializeField] private Entity GoalBlockPrefab;
    [SerializeField] private int MinBlockCount = 10;
    [SerializeField] private int MaxBlockCount = 30;
    [SerializeField] private float MinBlockSpawnHeight = -1.5f;
    [SerializeField] private float MaxBlockSpawnHeight = -1f;
    [SerializeField] private float BlockSpawnDistance = 1f;
    

    [Header("System")]
    private GameManager _manager;
    public List<Entity> SpawnedBlocks;
    private int totalBlockCount;

    public void InitializeData()
    {
        _manager = GameManager.instance;
    }

    public void BuildingBlocks()
    {
        if (BlockList == null) return;

        totalBlockCount = Random.Range(MinBlockCount, MaxBlockCount);

        SpawnedBlocks.Add(_manager.SpawnObject<Entity>(StartingBlockPrefab, StartingBlockPosition));


        for (int i = 1; i < totalBlockCount; i++)
        {
            SpawnedBlocks.Add(_manager.SpawnObject<Entity>(BlockList[Random.Range(0, BlockList.Count)], new Vector3(SpawnedBlocks[i - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight), 0)));

        }

        SpawnedBlocks.Add(_manager.SpawnObject<Entity>(GoalBlockPrefab, new Vector3(SpawnedBlocks[totalBlockCount - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight))));

        foreach (var blocks in SpawnedBlocks)
        {
            _manager.SpawnedObjects.Add(blocks);
            blocks.InitializeData(_manager);
        }

    }

    public int GetTotalBlockCount()
    {
        return totalBlockCount;
    }
}
