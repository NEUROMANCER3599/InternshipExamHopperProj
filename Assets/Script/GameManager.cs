using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public enum GameState
{
    NONE,
    RUNNING,
    COMPLETED
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Gameplay Manager")]
    [SerializeField] public int Score;
    public int TimeWon = 0;

    [Header("Prefab Components")]
    [SerializeField] private Entity PlayerPrefab;


    [Header("System")]
    public GameState _GameState = GameState.NONE;
    [SerializeField] private Entity _player;
    [SerializeField] private CinemachineCamera _VirtualCam;
    public List<Entity> SpawnedObjects = new List<Entity>();
    [SerializeField] private KeyCode RestartKey = KeyCode.R;
    public BlockBuilder _BlockBuilder;
    public EntitySpawner _EntitySpawner;
    [SerializeField] private ConfinerBehavior CamConfiner;
    [SerializeField] private AcidBehavior AcidFloor;
    private CoreInputControl _CoreInputControl;
    private UIManager _UIManager;
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

        if (_BlockBuilder != null) return;
        _BlockBuilder = GetComponent<BlockBuilder>();

        if(_EntitySpawner != null) return;
        _EntitySpawner = GetComponent<EntitySpawner>();

        if(CamConfiner != null) return;
        CamConfiner = FindAnyObjectByType<ConfinerBehavior>();

        if(AcidFloor != null) return;
        AcidFloor = FindAnyObjectByType<AcidBehavior>();

        if(_CoreInputControl != null) return;
        _CoreInputControl = GetComponent<CoreInputControl>();

        if(_UIManager != null) return;
        _UIManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        _BlockBuilder.InitializeData(this);
        _EntitySpawner.InitializeData(this,_BlockBuilder);
        InitializeLevel();
    }

    void InitializeLevel() //Do not change this sequence
    {

        _BlockBuilder.BuildingBlocks(); //This must be first

        SpawningPlayer();

        AcidFloor.Initialize(this);

        CamConfiner.InitializeData(this);

        _UIManager.InitializeData(this);

        _CoreInputControl.InitializeData(this);

        _EntitySpawner.SpawningEntity();

        _GameState = GameState.RUNNING;
    }

    

    void Update()
    {

        

        if(_GameState == GameState.RUNNING) { CamConfiner.FollowingPlayer();}

        AcidFloor.UpdateData(GetPlayerRef());
        _CoreInputControl.UpdateData(GetPlayerRef());
        _UIManager.UpdateData(GetPlayerRef());

        if (Input.GetKeyDown(RestartKey))
        {
            if (!IsLose) return;
            OnRestart();
        }

        foreach(var Entity in SpawnedObjects)
        {
            if(Entity == null) SpawnedObjects.Remove(Entity);
            Entity.UpdateData();
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
        entity.InitializeData(this);
    }

    public void OnWin()
    {
        TimeWon++;
        ClearSpawnedObj();
        InitializeLevel();
    }

    public void OnLose()
    {
        IsLose = true;
    }

    public void OnRestart()
    {
        IsLose = false;
        TimeWon = 0;
        Score = 0;
        ClearSpawnedObj();
        InitializeLevel();
    }

    private void SpawningPlayer()
    {
        if (PlayerPrefab == null) return;
        _player = SpawnObject<Entity>(PlayerPrefab, Vector3.zero);
        SpawnedObjects.Add(_player);
        _player.InitializeData(this);
        _VirtualCam.Follow = _player.gameObject.transform;
    }

    private void ClearSpawnedObj()
    {
        _GameState = GameState.COMPLETED;
        foreach (var obj in SpawnedObjects)
        {
            Destroy(obj.gameObject);
        }
        SpawnedObjects.Clear();
        _BlockBuilder.SpawnedBlocks.Clear();
    }

    public T PickingUniqueItem<T>(List<T> itemlist,int MinIndex,int MaxIndex)
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
        return _player as PlayerBehavior;
    }

    public byte floatToByte(float f)
    {
        return (byte)(f * 255);
    }

    public bool LoseCheck()
    {
        return IsLose;
    }

}
