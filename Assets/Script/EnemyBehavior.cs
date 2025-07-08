using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EnemyBehavior : Entity
{
    [Header("Parameters")]
    [SerializeField] private int LifePoint = 3;
    [SerializeField] private float AttackDistance = 4;
    [SerializeField] private float AttackSpeed = 1;
    [SerializeField] private Entity ProjectilePrefab;
    [SerializeField] private Transform FirePoint;

    [Header("Animation Parameters")]
    private Animator _animator;
    [SerializeField] private string HitAnimationTrigger = "OnHit";
    [SerializeField] private string DeathAnimationTrigger = "OnDeath";
    [SerializeField] private string AttackAnimationTrigger = "OnAttack";

    [Header("Sounds")]
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip HitSound;
    [SerializeField] private AudioClip DeathSound;

    [Header("System")]
    [SerializeField] private PlayerBehavior _player;
    private bool IsDead;
    private bool IsAttacking;
    private SpriteRenderer _spriteRenderer;
    public override void InitializeData(GameManager GM)
    {

        base.InitializeData(GM);

        if(_animator == null)
       _animator = GetComponent<Animator>();

       _spriteRenderer = GetComponent<SpriteRenderer>();
       _gameManager = GameManager.instance;
    }

    public override void UpdateData()
    {
        if (_player == null) _player = _gameManager.GetPlayerRef();
        AttackCheck();
    }

    void AttackCheck()
    {
        if(_player == null) return;

        if (IsDead) return;

        if (Vector2.Distance(transform.position, _player.transform.position) > AttackDistance) return;

        if(IsAttacking) return;

        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        IsAttacking = true;
        _animator.SetTrigger(AttackAnimationTrigger);
        yield return new WaitForSeconds(AttackSpeed);
        IsAttacking = false;
    }

    public void FiringProjectile()
    {
        SoundFXManager.instance.PlaySoundFXClip(AttackSound, gameObject.transform);
        var Proj = _gameManager.SpawnObject<Entity>(ProjectilePrefab, FirePoint.position);
        Proj.InitializeData(_gameManager);
        _gameManager.AddEntity(Proj);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == null) return;
        if(collision.gameObject == _player.gameObject)
        {
            if(IsDead) return;
            _player.OnDamaged(1);
            OnDeath();
        }
    }

    public void OnDamaged()
    {
        if(IsDead) return;
        LifePoint--;
        _animator.SetTrigger(HitAnimationTrigger);
        SoundFXManager.instance.PlaySoundFXClip(HitSound, gameObject.transform);
        if (LifePoint <= 0) OnDeath();
    }

    void OnDeath()
    {
        IsDead = true;
        SoundFXManager.instance.PlaySoundFXClip(DeathSound, gameObject.transform);
        StopAllCoroutines();
        _animator.SetTrigger(DeathAnimationTrigger);
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
