using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Health Bar Components")]
    [SerializeField] private Image HealthBarObj;
    [SerializeField] private List<Sprite> HPsprite;

    [Header("System")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerBehavior _player;
    public void InitializeData(GameManager GM)
    {
        _gameManager = GM;
    }

    public void UpdateData()
    {
        if(_player == null) _player = _gameManager.GetPlayerRef();
        if(_player == null) return;

        HealthBarUpdate(_player.LifePoint);

    }

    public void HealthBarUpdate(int currentHP)
    {
        if (HPsprite[currentHP] == null) return;

        HealthBarObj.sprite = HPsprite[currentHP];
    }
}
