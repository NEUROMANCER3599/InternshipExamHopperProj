using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float MinSpikeActivateInterval = 4f;
    public float MaxSpikeActivateInterval = 8f;
    float SpikeActivateInterval;
    [SerializeField] private PlayerBehavior _player;
    private BoxCollider2D Col;
    private Animator _animator;
    bool IsActivated;
    private void Start()
    {
       InitializeData();
    }

    void InitializeData()
    {
        Col = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        Col.enabled = false;
    }

    private void Update()
    {
        if(_player == null) _player = FindAnyObjectByType<PlayerBehavior>();
        if (IsActivated) return;
        StartCoroutine(SpikeActivation());
    }

    IEnumerator SpikeActivation()
    {
        IsActivated = true;
        _animator.SetTrigger("OnActivate");
        yield return new WaitForSeconds(SpikeActivateInterval = Random.Range(MinSpikeActivateInterval,MaxSpikeActivateInterval));
        IsActivated = false;
    }

    public void SpikeActivated()
    {
        Col.enabled = true;
    }

    public void SpikeDeactivate()
    {
        Col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == null) return;
        if (collision.gameObject == _player.gameObject) { _player.OnDamaged(1); return; }
    }

}
