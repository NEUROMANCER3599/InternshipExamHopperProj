
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;


public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private SoundSourceObj soundFXObject;

    [Header("Pooling Parameters")]
    public int defaultCapacity = 10;
    public int maxSize = 50;

    private Transform DefaultTransform;

    private IObjectPool<SoundSourceObj> m_Pool;

    public IObjectPool<SoundSourceObj> SoundFxPool
    {
        get
        {
            if (m_Pool == null)
            {
                // Create the pool if it doesn't exist
                m_Pool = new ObjectPool<SoundSourceObj>(
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

    public SoundSourceObj CreatePooledItem()
    {
        SoundSourceObj sfx = GameManager.instance.SpawnObject<SoundSourceObj>(soundFXObject, transform.position);
        sfx.InitializeData(GameManager.instance);
        sfx.SetPool(SoundFxPool);
        GameManager.instance.AddEntity(sfx);
        return sfx;
    }

    void OnGetFromPool(SoundSourceObj Sfx_Obj)
    {
        Sfx_Obj.gameObject.SetActive(true);
    }

    void OnReleaseToPool(SoundSourceObj Sfx_Obj)
    {
        Sfx_Obj.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(SoundSourceObj Sfx_Obj)
    {
        Destroy(Sfx_Obj.gameObject);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform SoundPosition)
    {

        SoundSourceObj sfxobj = CreatePooledItem();

        sfxobj.SetClip(audioClip);

        sfxobj.playSound(SoundPosition);

    }

  
}
