using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;




public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Prefab Components")]
    [SerializeField] private List<GameObject> BlockList;
    [SerializeField] private List<GameObject> EntityList;
    [SerializeField] private GameObject PlayerPrefab;

    [Header("Block Spawn Parameters")]
    public Vector3 StartingBlockPosition = new Vector3(0,-1.5f,0);
    [SerializeField] private GameObject StartingBlockPrefab;
    [SerializeField] private GameObject GoalBlockPrefab;
    [SerializeField] private int MinBlockCount = 10;
    [SerializeField] private int MaxBlockCount = 30;
    [SerializeField] private float MinBlockSpawnHeight = -2.5f;
    [SerializeField] private float MaxBlockSpawnHeight = 0.5f;
    [SerializeField] private float BlockSpawnDistance = 1f;
    private int totalBlockCount;

    [Header("Entity Spawn Parameters")]
    [SerializeField] private float EntitySpawnHeightOffset = 0.5f;
    [SerializeField][Range(0,1)] private float EntityPerBlockPercentage = 0.65f;


    [Header("System")]
    public List<GameObject> SpawnedBlocks;
    [SerializeField] private PlayerBehavior _player;
    [SerializeField] private CinemachineCamera _VirtualCam;
    public List<GameObject> SpawnedObjects;
    public List<Transform> PossibleEntitySpawnPositions;
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

        if (_VirtualCam != null) return;

        _VirtualCam = GameObject.FindAnyObjectByType<CinemachineCamera>();
    }

    private void Start()
    {
        InitializeLevel();
    }

    void InitializeLevel() //Do not change this sequence
    {
       

        BuildingBlocks(); //This must be first

        SpawningEntity();

        SpawningPlayer();


    }

   
    
    void Update()
    {
        
    }

    void SpawningEntity()
    {
        if(EntityList == null) return;
        if(PossibleEntitySpawnPositions != null) PossibleEntitySpawnPositions.Clear();

        foreach(var blocks in SpawnedBlocks)
        {
            PossibleEntitySpawnPositions.Add(blocks.transform);
        }

        int totalEntityCount = Mathf.RoundToInt(totalBlockCount * EntityPerBlockPercentage);

        for(int i = 1; i < totalEntityCount; i++)
        {
            SpawnedObjects.Add(SpawnObject(EntityList[Random.Range(0, EntityList.Count)], PickingUniqueItem<Transform>(PossibleEntitySpawnPositions,1, PossibleEntitySpawnPositions.Count - 1).position + new Vector3(0,EntitySpawnHeightOffset,0)));
        }
    }

    void BuildingBlocks()
    {
        if (BlockList == null) return;

        totalBlockCount = Random.Range(MinBlockCount, MaxBlockCount);

        SpawnedBlocks.Add(SpawnObject(StartingBlockPrefab, StartingBlockPosition));

        for (int i = 1; i < totalBlockCount; i++)
        {
            SpawnedBlocks.Add(SpawnObject(BlockList[Random.Range(0, BlockList.Count)], new Vector3(SpawnedBlocks[i - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight), 0)));
        }

        SpawnedBlocks.Add(SpawnObject(GoalBlockPrefab, new Vector3(SpawnedBlocks[totalBlockCount - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight))));

    }

    private GameObject SpawnObject(GameObject SpawnPrefab, Vector3 SpawnLocation)
    {
        if (SpawnPrefab == null) return null;
        var SpawnedObj = Instantiate(SpawnPrefab, SpawnLocation, Quaternion.identity);
        SpawnedObjects.Add(SpawnedObj);
        return SpawnedObj;
    }

    public void OnWin()
    {
        ClearSpawnedObj();
        InitializeLevel();
    }

    private void SpawningPlayer()
    {
        if (PlayerPrefab == null) return;
        var playerInstance = SpawnObject(PlayerPrefab,Vector3.zero);
        _player = playerInstance.GetComponent<PlayerBehavior>();
        _player.InitializeData();
        _VirtualCam.Follow = _player.gameObject.transform;
    }

    private void ClearSpawnedObj()
    {
        foreach(var obj in SpawnedObjects)
        {
            Destroy(obj);
        }
        SpawnedBlocks.Clear();
        SpawnedObjects.Clear();
    }

    public static T PickingUniqueItem<T>(List<T> itemlist,int MinIndex,int MaxIndex)
    {
        if (itemlist == null) return default;

        if(MinIndex < 0) MinIndex = 0;
        if(MaxIndex >  itemlist.Count) MaxIndex = itemlist.Count;

        int index = Random.Range(MinIndex, MaxIndex);

        T Pickeditem = itemlist[index];
        itemlist.RemoveAt(index);

        return Pickeditem;
    }
}
