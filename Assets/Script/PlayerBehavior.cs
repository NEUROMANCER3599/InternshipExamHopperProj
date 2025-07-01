using UnityEngine;
using DG.Tweening;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Gameplay Data")]
    [SerializeField] private int LifePoint = 3;

    [Header("Animation Components")]
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private KeyCode JumpInput = KeyCode.Space;
    public float JumpPositionOffset = 0.375f;
    public float JumpPower = 5f;
    public float JumpSpeed = 0.5f;
    public Vector2 GroundCheckBox;
    public float GroundCheckDistance;
    public LayerMask GroundLayer;

    [Header("System")]
    [SerializeField] private int StepNum = 0;
    private bool IsActive = true;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void InitializeData()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        _animator = GetComponentInChildren<Animator>();
        gameManager = GameManager.instance;
    }

    public void UpdateData()
    {
        if (Input.GetKeyDown(JumpInput))
        {
            if (!GroundCheck()) return;
            if (!IsActive) return;
            Jump();
        }
    }

    private Vector3 GetJumpPosition()
    {
        Vector3 JumpPosition = gameManager.SpawnedBlocks[StepNum+1].transform.position + new Vector3(0,JumpPositionOffset,0);
        return JumpPosition;
    }

    public void Jump()
    {
        if (StepNum + 1 >= gameManager.SpawnedBlocks.Count) return;

        rb.DOJump(GetJumpPosition(), JumpPower, 1, JumpSpeed, false);
        _animator.SetTrigger("Jump");
        StepNum++;
       
    }

    public bool GroundCheck()
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
        LifePoint--;
        _animator.SetTrigger("OnHurt");
        if (LifePoint <= 0) LoseEvent();
    }

    public void LoseEvent()
    {
        
        IsActive = false;
        gameManager.OnLose();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position-transform.up * GroundCheckDistance, GroundCheckBox);
    }

    private void OnDestroy()
    {
        DOTween.KillAll(true);
    }
}
