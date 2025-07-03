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

    [Header("System")]
    [SerializeField] private PlayerBehavior _player;
    private bool IsDead;
    private bool IsAttacking;
    private GameManager _gameManager;
    public override void InitializeData()
    {
        if(_animator == null)
       _animator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(AttackSpeed);
        _animator.SetTrigger(AttackAnimationTrigger); 
        IsAttacking = false;
    }

    public void FiringProjectile()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == null) return;
        if(collision.gameObject == _player.gameObject)
        {
            if(IsDead) return;
            _player.OnDamaged();
            OnDeath();
        }
    }

    public void OnDamaged()
    {
        if(IsDead) return;
        LifePoint--;
        _animator.SetTrigger(HitAnimationTrigger);
        if(LifePoint <= 0) OnDeath();
    }

    void OnDeath()
    {
        IsDead = true;
        StopAllCoroutines();
        _animator.SetTrigger(DeathAnimationTrigger);
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
