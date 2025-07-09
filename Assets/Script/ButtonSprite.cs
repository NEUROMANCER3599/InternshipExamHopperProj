using UnityEngine;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
    [SerializeField] private Sprite IdleSprite;
    [SerializeField] private Sprite PressedSprite;

    private Image imgComponent;
    private void Awake()
    {
        imgComponent = GetComponent<Image>();
    }
    public void OnUnpressed()
    {
        if (imgComponent == null) return;
        imgComponent.sprite = IdleSprite;
    }

    public void OnPressed()
    {
        if (imgComponent == null) return;
        imgComponent.sprite = PressedSprite;
    }
}
