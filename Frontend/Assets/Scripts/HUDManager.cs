using UnityEngine;
using TMPro;
public class HUDManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text xpText;

    private PlayerHealth playerHealth;
    private PlayerXP playerXP;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerXP = player.GetComponent<PlayerXP>();
        }

        UpdateHUD();
    }

    void Update()
    {
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Health: " + playerHealth.currentHealth;
        }

        if (playerXP != null && xpText != null)
        {
            xpText.text = "XP: " + playerXP.currentXP;
        }
    }
}