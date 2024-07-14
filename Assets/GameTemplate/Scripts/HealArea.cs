using UnityEngine;
using DG.Tweening;
using System;

public class HealArea : MonoBehaviour
{
    [Foldout("Heal Area Visual", foldEverything = true, styled = true, readOnly = false)]
    public Shapes.Disc progressShape; 

    [Foldout("Heal Area Settings", foldEverything = true, styled = true, readOnly = false)]
    public float healWaitTime;
    public float healCooldown;
    public int healAmount;

    private bool isPlayerInside = false;
    private float healTimer = 0f;
    private float healCooldownTimer = 0f;

    private void Update()
    {
        if (isPlayerInside)
        {
            healTimer += Time.deltaTime;
            if(progressShape.AngRadiansEnd < 360)
            {
                float totalIncrease = 2 * Mathf.PI;
                float increasePerSecond = totalIncrease / healWaitTime;
                progressShape.AngRadiansEnd += increasePerSecond * Time.deltaTime;
            }
                
            if (healTimer >= healWaitTime)
            {
                healCooldownTimer += Time.deltaTime;
                if (healCooldownTimer >= healCooldown)
                {
                    doHealing();
                    healCooldownTimer = 0f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if(GameManager.Instance.player.currentHealth < GameManager.Instance.player.maxHealth)
            {
                isPlayerInside = true;
                healTimer = 0f; 
                healCooldownTimer = 0f; 
                progressShape.AngRadiansEnd = 0;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            healTimer = 0f; 
            healCooldownTimer = 0f; 
            progressShape.AngRadiansEnd = 0;
        }
    }

    private void doHealing()
    {
        GameManager.Instance.player.Heal(healAmount);
    }
}