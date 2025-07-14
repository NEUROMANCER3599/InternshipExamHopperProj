using UnityEngine;

public class AnimCleanup : Entity
{
    private Animator _animPlayer;
    private float _animTime;
    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
        _animPlayer = GetComponent<Animator>();

        AnimationClip[] clips = _animPlayer.runtimeAnimatorController.animationClips;

        foreach(AnimationClip clip in clips)
        {
            _animTime = clip.length;
        }

        Invoke(nameof(AutoCleanup), _animTime);

    }
   void AutoCleanup()
    {
        Destroy(gameObject);
    }
}
