using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMapDisplay : MonoBehaviour 
{
	[Header("UI Elements")]
	[SerializeField] private MapData mapData;
    [SerializeField] private TMP_Text mapTitle;
    [SerializeField] private Image mapImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Button button;

	private void OnEnable() {
		mapTitle.text = mapData.title;
		mapImage.sprite = mapData.image;
		lockImage.gameObject.SetActive(!mapData.isUnlocked);
		button.gameObject.SetActive(mapData.isUnlocked);
	}
}
