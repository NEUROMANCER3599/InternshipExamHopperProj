using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;




public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Gameplay Manager")]
    [SerializeField] private int Score;

    [Header("Prefab Components")]
    [SerializeField] private List<Entity> BlockList;
    [SerializeField] private List<Entity> EntityList;
    [SerializeField] private Entity PlayerPrefab;

    [Header("Block Spawn Parameters")]
    public Vector3 StartingBlockPosition = new Vector3(0,-1.5f,0);
    [SerializeField] private Entity StartingBlockPrefab;
    [SerializeField] private Entity GoalBlockPrefab;
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
    public List<Entity> SpawnedBlocks;
    [SerializeField] private Entity _player;
    [SerializeField] private CinemachineCamera _VirtualCam;
    public List<Entity> SpawnedObjects;
    public List<Transform> PossibleEntitySpawnPositions;
    [SerializeField] private KeyCode RestartKey = KeyCode.R;
    bool IsLose;
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

        SpawningPlayer();

        SpawningEntity();

        

    }

   
    
    void Update()
    {
        
        if (Input.GetKeyDown(RestartKey))
        {
            if (!IsLose) return;
            OnRestart();
        }

        if (SpawnedObjects == null) return;

        foreach(var Entity in SpawnedObjects)
        {
            if(Entity == null) SpawnedObjects.Remove(Entity);
            Entity.UpdateData();
        }

        
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
            var SpawnedEntity = SpawnObject<Entity>(EntityList[Random.Range(0, EntityList.Count)], PickingUniqueItem<Transform>(PossibleEntitySpawnPositions, 1, PossibleEntitySpawnPositions.Count - 1).position + new Vector3(0, EntitySpawnHeightOffset, 0));
            SpawnedObjects.Add(SpawnedEntity);
            SpawnedEntity.InitializeData();
        }
    }

    void BuildingBlocks()
    {
        if (BlockList == null) return;

        totalBlockCount = Random.Range(MinBlockCount, MaxBlockCount);

        SpawnedBlocks.Add(SpawnObject<Entity>(StartingBlockPrefab, StartingBlockPosition));
        

        for (int i = 1; i < totalBlockCount; i++)
        {
            SpawnedBlocks.Add(SpawnObject<Entity>(BlockList[Random.Range(0, BlockList.Count)], new Vector3(SpawnedBlocks[i - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight), 0)));
            
        }

        SpawnedBlocks.Add(SpawnObject<Entity>(GoalBlockPrefab, new Vector3(SpawnedBlocks[totalBlockCount - 1].transform.position.x + BlockSpawnDistance, Random.Range(MinBlockSpawnHeight, MaxBlockSpawnHeight))));

        foreach(var blocks in SpawnedBlocks)
        {
            SpawnedObjects.Add(blocks);
            blocks.InitializeData();
        }
       
    }

    public T SpawnObject<T>(T SpawnPrefab, Vector3 SpawnLocation) where T : MonoBehaviour
    {
        if (SpawnPrefab == null) return default;
        var SpawnedObj = Instantiate(SpawnPrefab, SpawnLocation, Quaternion.identity);
        return SpawnedObj;
    }

    public void AddEntity(Entity entity)
    {
        SpawnedObjects.Add(entity);
        entity.InitializeData();
    }

    public void OnWin()
    {
        
        ClearSpawnedObj();
        InitializeLevel();
    }

    public void OnLose()
    {
        IsLose = true;
    }

    public void OnRestart()
    {
        ClearSpawnedObj();
        InitializeLevel();
    }

    private void SpawningPlayer()
    {
        if (PlayerPrefab == null) return;
        _player = SpawnObject<Entity>(PlayerPrefab, Vector3.zero);
        SpawnedObjects.Add(_player);
        _player.InitializeData();
        _VirtualCam.Follow = _player.gameObject.transform;
    }

    private void ClearSpawnedObj()
    {
        foreach(var obj in SpawnedObjects)
        {
            Destroy(obj.gameObject);
        }
        SpawnedObjects.Clear();
        SpawnedBlocks.Clear();
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

    public void Scoring(int score)
    {
        Score += score;
    }

    public PlayerBehavior GetPlayerRef()
    {
        return _player.GetComponent<PlayerBehavior>();
    }
}
