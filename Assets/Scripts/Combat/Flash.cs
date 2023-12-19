using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public float FlashTime => _flashTime;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = .1f;

    private SpriteRenderer[] _spriteRenderers;
    private ColorChanger _colorChanger;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash()
    {
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.material = _whiteFlashMaterial;
            if (_colorChanger) { _colorChanger.SetColor(Color.white);}
        }

        yield return new WaitForSeconds(_flashTime);

        BackToDefaultMaterial();
    }

    private void BackToDefaultMaterial()
    {
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.material = _defaultMaterial;
            if (_colorChanger) { _colorChanger.SetColor(_colorChanger.DefaultColor); }
        }
    }
}
