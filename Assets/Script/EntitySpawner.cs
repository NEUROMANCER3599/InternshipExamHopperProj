using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("Entity Spawn Parameters")]
    [SerializeField] private List<Entity> EntityList;
    [SerializeField] private float EntitySpawnHeightOffset = 1f;
    [SerializeField][Range(0, 1)] private float EntityPerBlockPercentage = 0.5f;

    [Header("System")]
    public List<Transform> PossibleEntitySpawnPositions;
    private BlockBuilder _blockBuilder;
    private GameManager _gameManager;

    public void InitializeData(GameManager GM, BlockBuilder BB)
    {
        _blockBuilder = BB;
        _gameManager = GM;
    }
    public void SpawningEntity()
    {
        if (EntityList == null) return;
        if (PossibleEntitySpawnPositions != null) PossibleEntitySpawnPositions.Clear();

        foreach (var blocks in _blockBuilder.SpawnedBlocks)
        {
            PossibleEntitySpawnPositions.Add(blocks.transform);
        }

        int totalEntityCount = Mathf.RoundToInt(_blockBuilder.GetTotalBlockCount() * EntityPerBlockPercentage);

        for (int i = 1; i < totalEntityCount; i++)
        {
            var SpawnedEntity = _gameManager.SpawnObject<Entity>(EntityList[Random.Range(0, EntityList.Count)], _gameManager.PickingUniqueItem<Transform>(PossibleEntitySpawnPositions, 3, PossibleEntitySpawnPositions.Count - 1).position + new Vector3(0, EntitySpawnHeightOffset, 0));
            _gameManager.SpawnedObjects.Add(SpawnedEntity);
            SpawnedEntity.InitializeData(_gameManager);
        }
    }
}
