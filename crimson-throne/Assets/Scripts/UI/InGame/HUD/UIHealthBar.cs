using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    [SerializeField] private Image mask;
    private float originalSize;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            originalSize = mask.rectTransform.rect.width;  
        }  
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}