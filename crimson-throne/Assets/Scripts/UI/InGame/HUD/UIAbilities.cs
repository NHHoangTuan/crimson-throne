using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilities : MonoBehaviour
{
    public static UIAbilities instance { get; private set; }
    [SerializeField] private List<GameObject> buffsIcons;
    [SerializeField] private List<GameObject> skillsIcons;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddBuffIcon(Sprite artwork, int index)
    {
        buffsIcons[index].GetComponent<Image>().sprite = artwork;
        buffsIcons[index].GetComponent<CanvasGroup>().alpha = 1;
    }

    public void AddSkillIcon(Sprite artwork, int index)
    {
        skillsIcons[index].GetComponent<Image>().sprite = artwork;
        skillsIcons[index].GetComponent<CanvasGroup>().alpha = 1;
    }

    public void Reset()
    {
        foreach (GameObject icon in buffsIcons) 
        {
            if (icon != null)
            {
                Image image = icon.GetComponent<Image>();
                CanvasGroup canvasGroup = icon.GetComponent<CanvasGroup>();
                if (image != null) image.sprite = null;
                if (canvasGroup != null) canvasGroup.alpha = 0;
            }
        }

        foreach (GameObject icon in skillsIcons) 
        {
            if (icon != null)
            {
                Image image = icon.GetComponent<Image>();
                CanvasGroup canvasGroup = icon.GetComponent<CanvasGroup>();
                if (image != null) image.sprite = null;
                if (canvasGroup != null) canvasGroup.alpha = 0;
            }
        }
    }
}