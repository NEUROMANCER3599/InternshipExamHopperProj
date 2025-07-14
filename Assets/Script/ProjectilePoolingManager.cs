using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolingManager : MonoBehaviour
{
    public int defaultCapacity = 10;
    public int maxSize = 50;

    public Transform Firepoint;

    public Projectile ProjectilePrefab;

    private IObjectPool<Projectile> m_Pool;

    public IObjectPool<Projectile> ProjectilePool
    {
        get
        {
            if (m_Pool == null)
            {
                // Create the pool if it doesn't exist
                m_Pool = new ObjectPool<Projectile>(
                    CreatePooledItem,
                    OnGetFromPool,
                    OnReleaseToPool,
                    OnDestroyPooledObject,
                    true,
                    defaultCapacity,
                    maxSize);
            }
            return m_Pool;
        }
    }

    public Projectile CreatePooledItem()
    {
        Projectile bullet = GameManager.instance.SpawnObject<Projectile>(ProjectilePrefab, Firepoint.position);
        bullet.InitializeData(GameManager.instance);
        bullet.SetPool(ProjectilePool);
        GameManager.instance.AddEntity(bullet);
        return bullet;
    }

    void OnGetFromPool(Projectile P_Obj)
    {
        P_Obj.gameObject.SetActive(true);
    }

    void OnReleaseToPool(Projectile P_Obj)
    {
        P_Obj.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(Projectile P_Obj)
    {
        Destroy(P_Obj.gameObject);
    }

}
