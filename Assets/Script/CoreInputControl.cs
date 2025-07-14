using UnityEngine;

public class CoreInputControl : MonoBehaviour
{
    [SerializeField] private PlayerBehavior _player;
    [SerializeField] private GameManager _gameManager;
    public void InitializeData(GameManager GM)
    {
        _gameManager = GM;
    }
    public void UpdateData(PlayerBehavior P)
    {
        if (_player == null) _player = P;
    }

    public void OnJump()
    {
        if (_player == null) return;
        _player.Jump();

        if (_gameManager.LoseCheck()) 
        _gameManager.OnRestart();
    }

    public void OnAttack()
    {
        if (_player == null) return;
        _player.StartAttack();
    }

    public void OnDefend()
    {
        if (_player == null) return;
        _player.Defend(true);
    }

    public void OnUndefend()
    {
        if (_player == null) return;
        _player.Defend(false);
    }
}
