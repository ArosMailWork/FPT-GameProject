using PA;
using UnityEngine;

public class BuffItem : IteractableItem
{
    [SerializeField]
    private int healthBuff = 100;

    [SerializeField]
    private float speedBuff = 10f;

    [SerializeField]
    private int damageBuff = 10;

    private PlayerStatsManager playerStatsManager;

    private void Start()
    {
        base.Start();
        playerStatsManager = FindObjectOfType<PlayerStatsManager>();
        if (playerStatsManager == null)
        {
            Debug.LogError("PlayerStatsManager not found in the scene!");
        }
    }

    public override void OnActiveButtonClicked()
    {
        base.OnActiveButtonClicked();
        ApplyBuff();
    }

    private void ApplyBuff()
    {
        if (playerStatsManager != null)
        {
            playerStatsManager.BuffPlayer(healthBuff, speedBuff, damageBuff);
            Destroy(gameObject); // Remove the buff item after use
        }
        else
        {
            Debug.LogError("Cannot apply buff: PlayerStatsManager is null!");
        }
    }
}
