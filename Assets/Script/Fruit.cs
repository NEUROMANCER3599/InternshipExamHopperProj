using DG.Tweening.Core.Easing;
using UnityEngine;

public class Fruit : Entity
{
    public int HealthPoint = 1;
    private PlayerBehavior _player;
    public override void InitializeData()
    {
        
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
        _player.Healing(HealthPoint);
        Destroy(gameObject);
    }
}
