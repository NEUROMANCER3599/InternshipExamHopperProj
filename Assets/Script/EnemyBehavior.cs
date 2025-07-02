using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EnemyBehavior : Entity
{
    [Header("Parameters")]
    public int LifePoint = 3;

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
            _player.OnDamaged();
            OnDeath();
        }
    }

    public void OnDamaged()
    {
        Debug.Log("Enemy Hit!");
        LifePoint--;
        if(LifePoint <= 0) OnDeath();
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
