using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Entity))]
public class Coin : Entity
{
    public int ScorePoint = 1;
    private Animator _animator;
    private PlayerBehavior _player;
    private GameManager gameManager;
    public override void InitializeData()
    {
        _animator = GetComponent<Animator>();
        gameManager = GameManager.instance;
    }

    public override void UpdateData()
    {
        if (_player == null) _player = GameObject.FindAnyObjectByType<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player.gameObject)
        {
            OnCollected();
        }
    }

    void OnCollected()
    {
        gameManager.Scoring(ScorePoint);
        Destroy(gameObject);
    }

}
