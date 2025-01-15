using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDisplay : MonoBehaviour 
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private Image template;
    [SerializeField] private Text levelText;
    [SerializeField] private Text newLabelText;

	public void UpdateCardDisplay(Ability ability)
	{
        if (ability == null) return;
		
		int nextLevel = ability.GetNextLevel();
		nameText.text = ability.GetAbilityName();
		descriptionText.text = ability.GetDescription();
		artworkImage.sprite = ability.GetArtwork();
		levelText.text = nextLevel.ToString();
		newLabelText.text = nextLevel == 1 ? "New" : "";
		if (nextLevel == ability.GetMaxLevel())
		{
			Color goldenColor = new Color(1f, 0.843f, 0f);
    		template.color = goldenColor;
			newLabelText.text = "Max";
		}
	}
}