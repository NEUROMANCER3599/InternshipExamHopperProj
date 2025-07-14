using UnityEngine;
using DG.Tweening;
public class ConfinerBehavior : MonoBehaviour
{
    private PlayerBehavior _player;
    private GameManager _gameManager;
    public void InitializeData(GameManager GM)
    {
        _gameManager = GM;
        _player = _gameManager.GetPlayerRef();
    }

    public void FollowingPlayer()
    {
        if (_player == null) _player = _gameManager.GetPlayerRef();
        if (_player == null) return;

        transform.position = new Vector3(_player.transform.position.x, 0, 0);
    }
}
