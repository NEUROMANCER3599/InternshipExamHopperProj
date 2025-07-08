using UnityEngine;

public class Entity : MonoBehaviour
{
    public GameManager _gameManager;
    public virtual void InitializeData(GameManager GM){ _gameManager = GM; }
    public virtual void UpdateData() { }
}
