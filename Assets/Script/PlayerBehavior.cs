using UnityEngine;
using DG.Tweening;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private KeyCode JumpInput = KeyCode.Space;
    public float JumpPositionOffset = 0.375f;
    public float JumpPower = 5f;
    public float JumpSpeed = 0.5f;

    [Header("System")]
    [SerializeField] private int StepNum = 0;
    private Rigidbody2D rb;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void InitializeData()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(JumpInput))
        {
            Jump();
        }
    }

    private Vector3 GetJumpPosition()
    {
        Vector3 JumpPosition = gameManager.SpawnedBlocks[StepNum+1].transform.position + new Vector3(0,JumpPositionOffset,0);
        Debug.Log("Next Jump Position: " + JumpPosition);
        return JumpPosition;
    }

    void Jump()
    {
        if (StepNum + 1 >= gameManager.SpawnedBlocks.Count) return;

        rb.DOJump(GetJumpPosition(), JumpPower, 1, JumpSpeed, true);
        StepNum++;
       
    }
}
