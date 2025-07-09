using UnityEngine;
using DG.Tweening;
public class AcidBehavior : MonoBehaviour
{
    [Header("Acid Components")]
    public Vector3 AcidStartingPosition = new Vector3(0, -5, 0);
    public Vector3 AcidEndingPosition = new Vector3(0, 2, 0);
    [Range(0,1)]public float DefaultRisingTime = 0.1f;
    [Range(0,1)]public float MaxRisingTime = 1f;
    float RisingDuration;

    [Header("System")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerBehavior _player;

    private void Awake()
    {
        
    }
    public void Initialize()
    {
        if(_gameManager == null) _gameManager = GameManager.instance;
        transform.position = AcidStartingPosition;
        RisingDuration = DefaultRisingTime + (_gameManager.TimeWon * 0.01f);
        if(RisingDuration > MaxRisingTime) RisingDuration = MaxRisingTime;
    }

    public void UpdateData()
    {
        if(_player == null) _player = FindAnyObjectByType<PlayerBehavior>();

        if(transform.position.y < AcidEndingPosition.y)
        {
            transform.Translate(Vector3.up * (RisingDuration * 0.25f)  * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_player == null) return;
        if(collision.gameObject == _player.gameObject)
        {
            _player.OnDamaged(4);
        }
    }
}
