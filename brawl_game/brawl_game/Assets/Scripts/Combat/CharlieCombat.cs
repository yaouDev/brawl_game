using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharlieCombat : PlayerCombat
{
    public override void LightAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.light))
        {
            return;
        }

        AttackCooldown(AttackType.light);
        Attack_Raycast(longRange, baseDamage);
        PlayAttackSound(AttackType.light);
    }

    public override void HeavyAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.heavy))
        {
            return;
        }

        AttackCooldown(AttackType.heavy);
        Attack_Bomb();
        PlayAttackSound(AttackType.heavy);
    }

    public override void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(ctx, AttackType.special))
        {
            return;
        }

        AttackCooldown(AttackType.special);
        Attack_Instantiate();
        PlayAttackSound(AttackType.special);
    }
}
