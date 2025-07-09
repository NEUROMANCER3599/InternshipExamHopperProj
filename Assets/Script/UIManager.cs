using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Health Bar Components")]
    [SerializeField] private Image HealthBarObj;
    [SerializeField] private List<Sprite> HPsprite;

    [Header("GameOver Components")]
    [SerializeField] private List<GameObject> AllGameOverComponents;
    [SerializeField] private TextMeshProUGUI TotalScoreText;
    [SerializeField] private TextMeshProUGUI TotalLevelText;

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

        GameOverScreenCheck();
    }

    public void GameOverScreenCheck()
    {
            foreach(var items in AllGameOverComponents)
            {
                items.SetActive(_gameManager.LoseCheck());
            }

            TotalScoreText.text = "Total Score: " + _gameManager.Score.ToString();
            TotalLevelText.text = "Total Levels: " + _gameManager.TimeWon.ToString();
    }

    public void HealthBarUpdate(int currentHP)
    {
        if (HPsprite[currentHP] == null) return;

        HealthBarObj.sprite = HPsprite[currentHP];
    }
}
