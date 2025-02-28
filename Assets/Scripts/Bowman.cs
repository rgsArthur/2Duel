using UnityEngine;

public class Bowman : Character
{
    [Header("Настройки яда")]
    [Range(0, 1)] public float poisonChance = 0.4f;
    public float poisonDuration = 4f;
    public int poisonDamagePerTick = 3;
    public float tickInterval = 1f;

    private Character poisonedTarget;
    private float poisonTimer;
    private float nextTickTime;

    public override void Attack(Character target)
    {
        base.Attack(target);

        if (target.currentHealth > 0 && Random.value <= poisonChance)
        {
            poisonedTarget = target;
            poisonedTarget.statusText.text = currentDamage + ", Отравлен!";
            poisonTimer = poisonDuration;
            nextTickTime = 0;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (poisonedTarget == null || poisonedTarget.currentHealth <= 0) return;
        poisonTimer -= Time.deltaTime;
        nextTickTime -= Time.deltaTime;

        if (nextTickTime <= 0)
        {
            poisonedTarget.TakeDamage(poisonDamagePerTick);
            nextTickTime = tickInterval;
        }

        if (poisonTimer <= 0 && poisonedTarget.statusText != null)
        {
            poisonedTarget.statusText.text = "";
            poisonedTarget = null;
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        if (poisonedTarget != null && poisonedTarget.statusText != null)
        {
            poisonedTarget.statusText.text = "";
            poisonedTarget = null;
        }
        poisonTimer = 0;
    }
}