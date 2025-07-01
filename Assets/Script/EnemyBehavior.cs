using UnityEngine;

public class EnemyBehavior : Entity
{


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
        if(collision.gameObject == _player.gameObject)
        {
            //_player.OnDamaged();
            OnDeath();

        }
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
