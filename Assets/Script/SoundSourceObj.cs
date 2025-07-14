using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(AudioSource))]
public class SoundSourceObj : Entity
{
    public AudioSource m_AudioSource;
    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
        m_AudioSource = GetComponent<AudioSource>();
    }

    private IObjectPool<SoundSourceObj> pool;
    public void SetPool(IObjectPool<SoundSourceObj> pool)
    {
        this.pool = pool;
    }

    public void OnSoundFinished()
    {
        pool.Release(this);
    }

    public void SetClip(AudioClip clip)
    {
        m_AudioSource.clip = clip;
    }

    public void playSound(Transform soundposition)
    {
        if (m_AudioSource.clip == null) return;
        transform.position = soundposition.position;
        m_AudioSource.Play();

        float clipLength = m_AudioSource.clip.length;
        Invoke(nameof(OnSoundFinished), clipLength);
    }
}
