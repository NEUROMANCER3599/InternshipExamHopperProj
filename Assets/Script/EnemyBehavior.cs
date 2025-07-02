using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EnemyBehavior : Entity
{
    [Header("Parameters")]
    public int LifePoint = 3;
    public float AttackDistance = 4;
    public float AttackSpeed = 1;
    public GameObject ProjectilePrefab;

    [Header("Animation Parameters")]
    private Animator _animator;
    public string HitAnimationTrigger = "OnHit";
    public string DeathAnimationTrigger = "OnDeath";
    public string AttackAnimationTrigger = "IsAttacking";

    [Header("System")]
    [SerializeField] private PlayerBehavior _player;
    private bool IsDead;
    private bool IsAttacking;
    public override void InitializeData()
    {
        if(_animator == null)
       _animator = GetComponent<Animator>();
    }

    public override void UpdateData()
    {
        if (_player == null) _player = GameObject.FindAnyObjectByType<PlayerBehavior>();
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
