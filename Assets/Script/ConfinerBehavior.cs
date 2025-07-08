using UnityEngine;
using DG.Tweening;
public class ConfinerBehavior : MonoBehaviour
{
    private PlayerBehavior _player;
    public void InitializeData()
    {
        _player = FindAnyObjectByType<PlayerBehavior>();
    }

    public void FollowingPlayer()
    {
        if (_player == null) _player = FindAnyObjectByType<PlayerBehavior>();
        if (_player == null) return;

        //transform.DOMoveX(_player.transform.position.x, 0.5f, false);

        transform.position = new Vector3(_player.transform.position.x, 0, 0);
    }
}
