using UnityEngine;

public class Warrior : Character
{
    [Range(0, 1)] public float stunChance = 0.3f;
    public float stunDuration = 2f;
    private Character stunnedTarget;
    private float stunTimer; 

    public override void Attack(Character target)
    {
        base.Attack(target);

        if (target.currentHealth > 0 && Random.value <= stunChance)
        {
            stunnedTarget = target;
            stunnedTarget.attackTimer += stunDuration;
            stunnedTarget.statusText.text = currentDamage + ", ќглушЄн!";
            stunTimer = stunDuration;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0 && stunnedTarget != null && stunnedTarget.statusText != null)
            {
                stunnedTarget.statusText.text = "";
                stunnedTarget = null;
            }
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();

        if (stunnedTarget != null && stunnedTarget.statusText != null)
        {
            stunnedTarget.statusText.text = "";
            stunnedTarget = null;
        }
        stunTimer = 0;
    }
}