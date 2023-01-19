using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownBar : MonoBehaviour
{
    public Image mask;
    public Color activeColor;
    public Color inactiveColor;

    private float originalSize;
    private Image bar;

    // Update is called once per frame
    void Start()
    {
        bar = GetComponent<Image>();
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetActive(bool active)
    {
        bar.color = active ? activeColor : inactiveColor;
    }
}
