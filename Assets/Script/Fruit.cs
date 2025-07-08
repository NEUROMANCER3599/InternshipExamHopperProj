using DG.Tweening.Core.Easing;
using UnityEngine;

public class Fruit : Entity
{
    public int HealthPoint = 1;
    public AudioClip CollectSound;
    private PlayerBehavior _player;
    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
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
        _player.Healing(HealthPoint);
        Destroy(gameObject);
    }
}
