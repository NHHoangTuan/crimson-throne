using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour
{
    public static LevelUpController instance { get; private set; }
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform layoutGroup;
    private System.Random random = new System.Random();
    private List<GameObject> activeCards = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private List<Ability> GetRandomAbilities()
    {
        List<Ability> selectedAbilities = new List<Ability>();

        List<Ability> activeSkills = SkillsManager.instance.activeSkills;
        List<Ability> inactiveSkills = SkillsManager.instance.inactiveSkills;
        List<Ability> activeBuffs = BuffsManager.instance.activeBuffs;
        List<Ability> inactiveBuffs = BuffsManager.instance.inactiveBuffs;

        int upgradableSkillsCount = SkillsManager.instance.GetUpgradableCount();
        int randomSkillCount = Random.Range(0, 4); 
        int skillToTake = Mathf.Min(randomSkillCount, upgradableSkillsCount, inactiveSkills.Count + activeSkills.Count);

        int randomBuffToTake = 3 - skillToTake;
        int upgradableBuffsCount = BuffsManager.instance.GetUpgradableCount();
        int buffToTake = Mathf.Min(randomBuffToTake, upgradableBuffsCount, inactiveBuffs.Count + activeBuffs.Count);
        
        if (buffToTake < randomBuffToTake)
        {   
            skillToTake = Mathf.Min(3 - buffToTake, upgradableSkillsCount, inactiveSkills.Count + activeSkills.Count);
        }

        List<Ability> combinedSkills;
        if (SkillsManager.instance.IsFull())
        {
            combinedSkills = activeSkills;
        }
        else
        {
            combinedSkills = inactiveSkills.Concat(activeSkills).ToList();
        }
        selectedAbilities.AddRange(combinedSkills.OrderBy(_ => random.Next()).Take(skillToTake).ToList());
        List<Ability> combinedBuffs;
        if (BuffsManager.instance.IsFull())
        {
            combinedBuffs = activeBuffs;
        }
        else
        {
            combinedBuffs = inactiveBuffs.Concat(activeBuffs).ToList();
        }
        selectedAbilities.AddRange(combinedBuffs.OrderBy(_ => random.Next()).Take(buffToTake).ToList());
        
        return selectedAbilities.OrderBy(_ => random.Next()).ToList();;
    }

    public void ShowLevelUpUI(int count)
    {
        if (count == 0)
        {
            Close();
            return;
        }
        PauseGame();
        UIManager.instance.levelUpPanel.SetActive(true);
        List<Ability> abilities = GetRandomAbilities();
        foreach (var ability in abilities)
        {
            GameObject card = Instantiate(cardPrefab, layoutGroup);
            activeCards.Add(card);

            UICardDisplay cardDisplay = card.GetComponent<UICardDisplay>();
            if (cardDisplay == null) continue;
            
            cardDisplay.UpdateCardDisplay(ability);

            Button levelUpButton = card.GetComponentInChildren<Button>();
            if (levelUpButton == null) continue;
            
            levelUpButton.onClick.AddListener(() =>
            {
                ClearCards();
                ability.LevelUp();
                ShowLevelUpUI(count - 1);
            });            
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void Close()
    {
        UIManager.instance.levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        ClearCards();
    }

    private void ClearCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();
    }
}