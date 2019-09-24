using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutLine : MonoBehaviour
{
    public Color color = Color.white;

    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    private void OnDisable()
    {
        UpdateOutline(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
