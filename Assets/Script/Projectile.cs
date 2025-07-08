using UnityEngine;
using DG.Tweening;
using System.Collections;
public class Projectile : Entity
{
    [Header("Parameters")]
    public float ProjectileSpeed = 1f;
    public float DespawnTime = 3;
    public AudioClip DespawnSound;

    [Header("System")]
    [SerializeField] private PlayerBehavior _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D rb;
    bool isActivated;

    private void Awake()
    {
        InitializeData(GameManager.instance);
    }

    public override void InitializeData(GameManager gameManager)
    {
        base.InitializeData(gameManager);
        _player = _gameManager.GetPlayerRef();
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.DOMove(_player.transform.position, ProjectileSpeed * Vector2.Distance(transform.position, _player.transform.position), false);
        Invoke(nameof(OnHit), DespawnTime);
    }

    public override void UpdateData()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_player == null) return;
        if (collision.gameObject == _player.gameObject) { _player.OnDamaged(1); OnHit(); return; }
    }

    public void OnHit()
    {
        _animator.SetTrigger("OnHit");
        SoundFXManager.instance.PlaySoundFXClip(DespawnSound,gameObject.transform);
    }

    public void SelfCleanup()
    {
        DOTween.Kill(gameObject, true);
        Destroy(gameObject);
    }

}
