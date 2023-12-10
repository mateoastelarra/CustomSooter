using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = .1f;

    private SpriteRenderer[] _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
            renderer.color = Color.white;
        }

        yield return new WaitForSeconds(_flashTime);

        BackToDefaultMaterial();
    }

    private void BackToDefaultMaterial()
    {
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.material = _defaultMaterial;
        }
    }
}
