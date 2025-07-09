using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Entity))]
public class PlayerBehavior : Entity
{

    [Header("Gameplay Data")]
    [SerializeField] private int LifePoint = 3;

    [Header("Animation Components")]
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private KeyCode JumpInput = KeyCode.Space;
    [SerializeField] private KeyCode AttackInput = KeyCode.E;
    [SerializeField] private KeyCode DefendInput = KeyCode.F;
    public Transform AttackPoint;
    public float AttackHitBoxRadius = 1f;
    public float JumpPositionOffset = 0.375f;
    public float JumpPower = 5f;
    public float JumpSpeed = 0.5f;
    public Vector2 GroundCheckBox;
    public float GroundCheckDistance;
    public LayerMask GroundLayer;
    public LayerMask EnemyLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip HitSound;
    [SerializeField] private AudioClip DefendSound;
    [SerializeField] private AudioClip DefendHitSound;
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip DeathSound;

    [Header("System")]
    [SerializeField] private int StepNum = 0;
    private bool IsActive = true;
    private bool IsAttacking;
    private bool IsDefending;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    public override void UpdateData()
    {
            
            if (!GroundCheck()) return;
            if (!IsActive) return;
            if(IsAttacking) return;

            if(Input.GetKeyDown(DefendInput)) Defend(true);

            if(Input.GetKeyUp(DefendInput)) Defend(false);

            if (IsDefending) return;

            if(Input.GetKeyDown(JumpInput)) Jump();

            if(Input.GetKeyDown(AttackInput)) StartAttack();
      
    }

    private Vector3 GetJumpPosition()
    {
        Vector3 JumpPosition = _gameManager._BlockBuilder.SpawnedBlocks[StepNum+1].transform.position + new Vector3(0,JumpPositionOffset,0);
        return JumpPosition;
    }

    public void Jump()
    {
        
            if (StepNum + 1 >= _gameManager._BlockBuilder.SpawnedBlocks.Count) return;
            SoundFXManager.instance.PlaySoundFXClip(JumpSound, gameObject.transform);
            rb.DOJump(GetJumpPosition(), JumpPower, 1, JumpSpeed, false);
            _animator.SetTrigger("Jump");
            StepNum++;
        
    }

    public void StartAttack()
    {
            if (IsAttacking) return;
            IsAttacking = true;
            SoundFXManager.instance.PlaySoundFXClip(AttackSound, gameObject.transform);
            int RandomAnimIndex = Random.Range(0, 3);
            _animator.SetInteger("AttackAnimIndex", RandomAnimIndex);
            _animator.SetTrigger("OnAttack");
    }

    public void Defend(bool check)
    {
        IsDefending = check;

        if(IsDefending) SoundFXManager.instance.PlaySoundFXClip(DefendSound, gameObject.transform);

        _animator.SetBool("IsDefending",IsDefending);
    }

    private bool GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, GroundCheckBox, 0, -transform.up,GroundCheckDistance,GroundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            _gameManager.OnWin();
        }
    }

    public void OnDamaged(int DMG)
    {
        if(!IsActive) 
            return;
        if(IsDefending) SoundFXManager.instance.PlaySoundFXClip(DefendHitSound, gameObject.transform);

        if (IsDefending) return;
        LifePoint-= DMG;
        _animator.SetTrigger("OnHurt");
        SoundFXManager.instance.PlaySoundFXClip(HitSound, gameObject.transform);

        if (LifePoint <= 0) 
            LoseEvent();
    }

    private void LoseEvent()
    {
        
        IsActive = false;
        SoundFXManager.instance.PlaySoundFXClip(DeathSound, gameObject.transform);
        _animator.SetTrigger("OnDeath");
        _gameManager.OnLose();
    }

    private void AttackHit()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(AttackPoint.position,AttackHitBoxRadius,EnemyLayer);

        foreach (Collider2D collidedEnemy in enemy)
        {
            if (!collidedEnemy.gameObject.GetComponent<EnemyBehavior>()) return;
            EnemyBehavior EnemyInstance = collidedEnemy.gameObject.GetComponent<EnemyBehavior>();
            EnemyInstance.OnDamaged();
        }
    }

    private void endAttack()
    {
        IsAttacking = false;
    }

    public void Healing(int amount)
    {
        if (LifePoint == 3) return;
        LifePoint += amount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position-transform.up * GroundCheckDistance, GroundCheckBox);
        Gizmos.DrawSphere(AttackPoint.position, AttackHitBoxRadius);
    }

    private void OnDestroy()
    {
        DOTween.KillAll(true);
    }

}
