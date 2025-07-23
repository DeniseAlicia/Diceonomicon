using UnityEngine;

public class DiceController : MonoBehaviour
{
    public string nameText;
    public string descText;
    public Renderer textureRenderer;

    public void SetData(DiceData dieData)
    {
        nameText = dieData.name;
        descText = dieData.desc;
        textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
    }
}
