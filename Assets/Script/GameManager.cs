using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Prefab Components")]
    [SerializeField] private List<GameObject> BlockList;

    [Header("Block Spawn Parameters")]
    public Vector3 StartingBlockPosition = new Vector3(0,-1.5f,0);
    [SerializeField] private GameObject StartingBlockPrefab;
    [SerializeField] private int MinBlockCount = 10;
    [SerializeField] private int MaxBlockCount = 30;
    [SerializeField] private float MinBlockSpawnHeight = -2.5f;
    [SerializeField] private float MaxBlockSpawnHeight = 0.5f;
    [SerializeField] private float BlockSpawnDistance = 1f;
    private int totalBlockCount;

    [Header("System")]
    public List<GameObject> SpawnedBlocks;
    //public List<GameObject> SpawnedObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        InitializeData();
        BuildingBlocks();
    }

    void InitializeData()
    {
        foreach (var Player in FindObjectsByType<PlayerBehavior>(FindObjectsSortMode.None))
        {
            Player.InitializeData();
        }
    }

    void BuildingBlocks()
    {
        totalBlockCount = Random.Range(MinBlockCount, MaxBlockCount);

        SpawnedBlocks.Add(SpawnObject(StartingBlockPrefab, StartingBlockPosition));

        for (int i = 1; i < totalBlockCount; i++)
        {
            SpawnedBlocks.Add(SpawnObject(BlockList[Random.Range(0, BlockList.Count)], new Vector3(SpawnedBlocks[i-1].transform.position.x + BlockSpawnDistance,Random.Range(MinBlockSpawnHeight,MaxBlockSpawnHeight),0)));
        }
    }

    private GameObject SpawnObject(GameObject SpawnPrefab, Vector3 SpawnLocation)
    {
        var SpawnedObj = Instantiate(SpawnPrefab,SpawnLocation,Quaternion.identity);
        return SpawnedObj;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
