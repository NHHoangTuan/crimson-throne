using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityIcon : MonoBehaviour
{
    public static UIAbilityIcon instance { get; private set; }
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
}
