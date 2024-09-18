using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] protected GameObject healthBar;
    [SerializeField] protected Image healthBarFilling;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] protected Camera playerCamera;
    [SerializeField] private Enemy enemy;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateValue();
        healthBar.transform.LookAt(new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z));    
    }

    public virtual void UpdateValue() 
    {
        healthText.text = enemy.health.ToString();
        healthBarFilling.fillAmount = (float)enemy.health / enemy.maxHealth;
    }
}