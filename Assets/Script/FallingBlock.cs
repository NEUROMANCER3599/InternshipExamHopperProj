using DG.Tweening;
using UnityEngine;
using System.Collections;


public class FallingBlock : Entity
{
    [Header("Falling Block Parameters")]
    [SerializeField] private float MinTimeBeforeFalling = 5;
    [SerializeField] private float MaxTimeBeforeFalling = 10;
    [SerializeField] private AudioClip WarningSound;
    [SerializeField] private AudioClip BreakSound;
    [SerializeField] private AudioClip LandingSound;

    [Header("System")]
    private PlayerBehavior _player;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Collider2D _col;
    private bool IsTouched;
    private float TimeBeforeFalling;

    public override void InitializeData(GameManager GM)
    {
        base.InitializeData(GM);
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        TimeBeforeFalling = Random.Range(MinTimeBeforeFalling, MaxTimeBeforeFalling);
    }

    public override void UpdateData()
    {
        if (_gameManager._GameState == GameState.RUNNING) _player = _gameManager.GetPlayerRef();
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTouched)
            return;
        if (collision.gameObject == _player.gameObject)
        {
            SoundFXManager.instance.PlaySoundFXClip(LandingSound, gameObject.transform);
            StartCoroutine(fallingblockseq()); 
        }
    }

    IEnumerator fallingblockseq()
    {
        IsTouched = true;
        yield return new WaitForSeconds(TimeBeforeFalling * 0.75f);
        StartCoroutine(flashing());
        yield return new WaitForSeconds(TimeBeforeFalling * 0.25f);
        BlockFalling();
        StopAllCoroutines();
    }

    IEnumerator flashing()
    {
        _spriteRenderer.color = new Color32(_gameManager.floatToByte(1f), _gameManager.floatToByte(1f), _gameManager.floatToByte(1f), _gameManager.floatToByte(1));
        SoundFXManager.instance.PlaySoundFXClip(WarningSound, gameObject.transform);
        yield return new WaitForSeconds(TimeBeforeFalling * 0.025f);
        _spriteRenderer.color = new Color32(_gameManager.floatToByte(1f), _gameManager.floatToByte(1f), _gameManager.floatToByte(1f), _gameManager.floatToByte(0.75f));
        SoundFXManager.instance.PlaySoundFXClip(WarningSound, gameObject.transform);
        yield return new WaitForSeconds(TimeBeforeFalling * 0.025f);
        StartCoroutine(flashing());
    }

    void BlockFalling()
    {
        transform.DOMove(new Vector2(transform.position.x, transform.position.y - 5), 1f, false);
        SoundFXManager.instance.PlaySoundFXClip(BreakSound, gameObject.transform);
        if (_animator == null) return;
        _animator.SetTrigger("OnBreak");
        _col.enabled = false;
    }
    void OnBroken()
    {
        _spriteRenderer.enabled = false;
        DOTween.Kill(gameObject);
    }
}
