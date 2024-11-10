using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectRandomSkillEvent : MonoBehaviour
{
    #region Variables
    [FoldoutGroup("Setup/Buttons")]
    public Button SkillSelectSlot1, SkillSelectSlot2, SkillSelectSlot3;
    public Image SkillIMG1, SkillIMG2, SkillIMG3;

    [FoldoutGroup("Setup")]
    public List<GameObject> SkillPool = new List<GameObject>(); // All available skills
    [FoldoutGroup("Stats")]
    public List<GameObject> GachaSkillSlots = new List<GameObject>(); // The slots to choose from
    [FoldoutGroup("Stats")]
    public int selectedSlot; // The slot index for GachaSkillSlots
    [FoldoutGroup("Event")]
    public UnityEvent OnSkillChoose;
    public bool IsAdSlotEnable;

    PlayerData playerData;

    #endregion

    #region Methods

    [FoldoutGroup("Event")]
    [Button]
    public void PoolCreate()
    {
        // Clear the current SkillPool to refresh it with only unlocked skills
        SkillPool.Clear();

        if (playerData == null) playerData = PlayerController.Instance._playerData;

        HashSet<GameObject> addedSkills = new HashSet<GameObject>(); // To prevent duplicates

        // Add all default unlocked skills from the database
        foreach (var skill in playerData.skillDatabase.allSkills)
        {
            if (skill.defaultUnlocked && skill.skillPrefab != null && addedSkills.Add(skill.skillPrefab))
            {
                SkillPool.Add(skill.skillPrefab);
                // Debug.Log($"Added default unlocked skill: {skill.skillPrefab.name}");
            }
        }

        // Add unlocked skills to SkillPool
        foreach (SkillUnlock skillUnlock in playerData.unlockedSkills)
        {
            if (skillUnlock.isUnlocked)
            {
                // Find the corresponding skillPrefab from skillDatabase using the Skill_ID
                PlayerSkill playerSkill = playerData.skillDatabase.allSkills.Find(s => s.Skill_ID == skillUnlock.Skill.Skill_ID);
                if (playerSkill.skillPrefab != null && addedSkills.Add(playerSkill.skillPrefab))
                {
                    SkillPool.Add(playerSkill.skillPrefab);
                    Debug.Log($"Added {playerSkill.skillPrefab.name} to SkillPool.");
                }
                else
                {
                    Debug.LogWarning($"Skill prefab not found for {skillUnlock.Skill.Skill_ID}.");
                }
            }
        }

        Debug.Log("SkillPool created with unlocked skills.");
    }




    [FoldoutGroup("Event")]
    [Button]
    public void AddSelectSkillFromSlot()
    {
        if (selectedSlot >= 0 && selectedSlot < GachaSkillSlots.Count)
        {
            GameObject selectedSkill = GachaSkillSlots[selectedSlot];

            // Call SkillHolder.Instance.AddSkill() with the selected skill
            if (selectedSkill != null)
            {
                SkillHolder.Instance.AddSkill(selectedSkill);
                Debug.Log($"Skill from slot {selectedSlot} added: {selectedSkill.name}");
            }
            else
            {
                Debug.LogWarning($"No skill in the selected slot {selectedSlot}");
            }
        }
        else
        {
            Debug.LogError($"Invalid selectedSlot index: {selectedSlot}");
        }
    }

    [FoldoutGroup("Event")]
    [Button]
    public void GachaSkill(int amount)
    {
        // Clear previous GachaSkillSlots
        GachaSkillSlots.Clear();

        // Ensure we don't request more skills than are available in the SkillPool
        amount = Mathf.Clamp(amount, 1, SkillPool.Count);

        // Create a temporary copy of the SkillPool to avoid modifying the original list
        List<GameObject> tempSkillPool = new List<GameObject>(SkillPool);

        for (int i = 0; i < amount; i++)
        {
            // Select a random skill from the tempSkillPool
            int randomIndex = Random.Range(0, tempSkillPool.Count);
            GameObject selectedSkill = tempSkillPool[randomIndex];

            // Add the selected skill to GachaSkillSlots
            GachaSkillSlots.Add(selectedSkill);

            // Remove the selected skill from tempSkillPool to ensure uniqueness
            tempSkillPool.RemoveAt(randomIndex);
        }

        // Shuffle the positions in GachaSkillSlots to randomize their order
        for (int i = 0; i < GachaSkillSlots.Count; i++)
        {
            GameObject temp = GachaSkillSlots[i];
            int randomPos = Random.Range(i, GachaSkillSlots.Count);
            GachaSkillSlots[i] = GachaSkillSlots[randomPos];
            GachaSkillSlots[randomPos] = temp;
        }
    }
    [Button]
    public void SkillSelectStart()
    {
        GachaSkill(3);
        SkillIMG1.sprite = GachaSkillSlots[0].GetComponent<ISkill>().Icon;
        SkillIMG2.sprite = GachaSkillSlots[1].GetComponent<ISkill>().Icon;
        SkillIMG3.sprite = GachaSkillSlots[2].GetComponent<ISkill>().Icon;
    }

    public void SelectSkill(int choice)
    {
        if (choice < GachaSkillSlots.Count)
        {
            selectedSlot = choice;
            AddSelectSkillFromSlot();
            OnSkillChoose.Invoke();
        }


    }
    #endregion

}
