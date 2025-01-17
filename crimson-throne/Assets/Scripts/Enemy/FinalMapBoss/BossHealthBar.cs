using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    #region Singleton
    public static BossHealthBar instance { get; private set; }

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            originalSize = mask.rectTransform.rect.width;  
        }  
    }
    #endregion

    #region Variables
    [SerializeField] private Image mask;
    private float originalSize;
    #endregion

    #region Controls
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
    #endregion
}