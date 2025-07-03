using UnityEngine;
using DG.Tweening;
using System.Collections;
public class Projectile : Entity
{
    [Header("Parameters")]
    public float ProjectileSpeed = 1f;
    public float DespawnTime = 3;

    [Header("System")]
    [SerializeField] private PlayerBehavior _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D rb;

    private void OnEnable()
    {
        InitializeData();
    }
    public override void InitializeData()
    {

        if (_player == null) _player = FindAnyObjectByType<PlayerBehavior>();

        _animator = GetComponent<Animator>();
       
        rb = GetComponent<Rigidbody2D>();

        rb.DOMove(_player.transform.position, ProjectileSpeed * Vector2.Distance(transform.position, _player.transform.position), false);

        Invoke(nameof(OnHit),DespawnTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_player == null) return;
        if (collision.gameObject == _player.gameObject) { _player.OnDamaged(); OnHit();}
    }

    public void OnHit()
    {
        _animator.SetTrigger("OnHit");
    }

    public void SelfCleanup()
    {
        DOTween.Kill(gameObject, true);
        Destroy(gameObject);
    }

}
