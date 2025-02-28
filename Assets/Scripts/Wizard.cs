using UnityEngine;

public class Wizard : Character
{
    [Header("Настройки слабости")]
    [Range(0, 1)] public float weakeningChance = 0.25f;
    public float weakeningDuration = 3f;
    public float damageMultiplier = 0.5f;

    private Character weakenedTarget;
    private float weakenTimer;
    private int originalDamage;

    public override void Attack(Character target)
    {
        base.Attack(target);

        if (target.currentHealth > 0 && Random.value <= weakeningChance)
        {
            weakenedTarget = target;
            originalDamage = weakenedTarget.currentDamage;
            weakenedTarget.currentDamage = Mathf.RoundToInt(originalDamage * damageMultiplier);
            weakenedTarget.statusText.text = currentDamage + ", Ослаблен!";
            weakenTimer = weakeningDuration;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (weakenTimer > 0)
        {
            weakenTimer -= Time.deltaTime;
            if (weakenTimer <= 0 && weakenedTarget != null && weakenedTarget.statusText != null)
            {
                weakenedTarget.currentDamage = originalDamage;
                weakenedTarget.statusText.text = "";
                weakenedTarget = null;
            }
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        if (weakenedTarget != null && weakenedTarget.statusText != null)
        {
            weakenedTarget.currentDamage = originalDamage;
            weakenedTarget.statusText.text = "";
            weakenedTarget = null;
        }
        weakenTimer = 0;
    }
}