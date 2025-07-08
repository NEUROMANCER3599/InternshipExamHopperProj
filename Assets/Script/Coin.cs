using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Entity))]
public class Coin : Entity
{
    public int ScorePoint = 1;
    public AudioClip CollectSound;
    private Animator _animator;
    private PlayerBehavior _player;

    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);

        _animator = GetComponent<Animator>();

    }

    public override void UpdateData()
    {
        if (_player == null) _player = GameObject.FindAnyObjectByType<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == null) return;

        if (collision.gameObject == _player.gameObject)
        {
            OnCollected();
        }
    }

    void OnCollected()
    {
        SoundFXManager.instance.PlaySoundFXClip(CollectSound, gameObject.transform);
        _gameManager.Scoring(ScorePoint);
        _animator.SetTrigger("OnCollected");
    }

    void RemoveObject()
    {
        Destroy(gameObject);
    }

}
