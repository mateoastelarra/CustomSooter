using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }
    [SerializeField] private SpriteRenderer _fillSpriteRenderer;
    [SerializeField] private Color[] _colors;

    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }

    public void SetRandomColor()
    {
        int randomIndex = Random.Range(0, _colors.Length);
        SetDefaultColor(_colors[randomIndex]);
    }

}
