using UnityEngine;

public class HoverGlowController : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;
    public bool isHovered = false;
    public DiceSlotController thisSlot;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetHover(bool hover)
    {
        if (thisSlot.isFilled)
        {
            rend.GetPropertyBlock(mpb);
            mpb.SetFloat("_HoverTrigger", isHovered ? 1f : 0f);
            rend.SetPropertyBlock(mpb);
        }
        else if (isHovered == hover) return;
        isHovered = hover;

        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_HoverTrigger", isHovered ? 1f : 0f);
        rend.SetPropertyBlock(mpb);
    }
}
