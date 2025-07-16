using UnityEngine;

public class HoverGlowController : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;
    private bool isHovered = false;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void OnMouseEnter()
    {
        SetHover(true);
    }

    void OnMouseExit()
    {
        SetHover(false);
    }

    private void SetHover(bool hover)
    {
        if (isHovered == hover) return;
        isHovered = hover;

        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_HoverTrigger", isHovered ? 1f : 0f);
        rend.SetPropertyBlock(mpb);
    }
}
