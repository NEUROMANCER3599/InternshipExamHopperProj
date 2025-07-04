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

    [Header("System")]
    [SerializeField] private int StepNum = 0;
    private bool IsActive = true;
    private bool IsAttacking;
    private bool IsDefending;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void InitializeData()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        gameManager = GameManager.instance;
    }

    public override void UpdateData()
    {
            
            if (!GroundCheck()) return;
            if (!IsActive) return;
            if(IsAttacking) return;

            Defend();

            if (IsDefending) return;

            Jump();
            StartAttack();
      
    }

    private Vector3 GetJumpPosition()
    {
        Vector3 JumpPosition = gameManager._BlockBuilder.SpawnedBlocks[StepNum+1].transform.position + new Vector3(0,JumpPositionOffset,0);
        return JumpPosition;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(JumpInput))
        {
            if (StepNum + 1 >= gameManager._BlockBuilder.SpawnedBlocks.Count) return;

            rb.DOJump(GetJumpPosition(), JumpPower, 1, JumpSpeed, false);
            _animator.SetTrigger("Jump");
            StepNum++;
        }
    }

    private void StartAttack()
    {
        if (Input.GetKeyDown(AttackInput))
        {
            IsAttacking = true;
            int RandomAnimIndex = Random.Range(0, 3);
            _animator.SetInteger("AttackAnimIndex", RandomAnimIndex);
            _animator.SetTrigger("OnAttack");
        }
    }

    private void Defend()
    {
        if (Input.GetKeyDown(DefendInput))
        {
            IsDefending = true;
        }
        if (Input.GetKeyUp(DefendInput))
        {
            IsDefending = false;
        }

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
            gameManager.OnWin();
        }
    }

    public void OnDamaged()
    {
        if(!IsActive) return;
        if(IsDefending) return;
        LifePoint--;
        _animator.SetTrigger("OnHurt");
        if (LifePoint <= 0) LoseEvent();
    }

    private void LoseEvent()
    {
        
        IsActive = false;
        _animator.SetTrigger("OnDeath");
        gameManager.OnLose();
    }

    public void AttackHit()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(AttackPoint.position,AttackHitBoxRadius,EnemyLayer);

        foreach (Collider2D collidedEnemy in enemy)
        {
            if (!collidedEnemy.gameObject.GetComponent<EnemyBehavior>()) return;
            EnemyBehavior EnemyInstance = collidedEnemy.gameObject.GetComponent<EnemyBehavior>();
            EnemyInstance.OnDamaged();
        }
    }

    public void endAttack()
    {
        IsAttacking = false;
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
