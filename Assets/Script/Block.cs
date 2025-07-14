using System.Collections;
using UnityEngine;
using DG.Tweening;


public class Block : Entity
{

    [Header("Components")]
    public PlayerBehavior _player;
    public AudioClip LandingSound;

    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
    }

   
    public override void UpdateData()
    {
        if (_gameManager._GameState == GameState.RUNNING) _player = _gameManager.GetPlayerRef();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject == _player.gameObject)
        { SoundFXManager.instance.PlaySoundFXClip(LandingSound, gameObject.transform); }
    }

    
   
}
