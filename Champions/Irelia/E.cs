using System.Numerics;
using System.Collections.Generic;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;

namespace Spells
{
    public class IreliaEquilibriumStrike : GameScript
    {
        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            //spell.spellAnimation("SPELL3", owner);
        }

        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            spell.AddProjectileTarget("IreliaEquilibriumStrike", target, false);

        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
            float ownerPercent = owner.GetStats().CurrentHealth / owner.GetStats().HealthPoints.Total;
            float enemyPercent = target.GetStats().CurrentHealth / target.GetStats().HealthPoints.Total;
            float duration = new float[] { 1.0f, 1.25f, 1.5f, 1.75f, 2f }[spell.Level - 1];
            if (enemyPercent < ownerPercent)
            {
                

                var slow = ((ObjAIBase)target).AddBuffGameScript("IreliaSlow", "IreliaSlow", spell, -1, true);

                ApiFunctionManager.CreateTimer(duration, () =>
                {
                    owner.RemoveBuffGameScript(slow);
                });
            }
            else
            {

                var stun = ((ObjAIBase)target).AddBuffGameScript("Stun", "Stun", spell, -1, true);

                ApiFunctionManager.CreateTimer(duration, () =>
                {
                    owner.RemoveBuffGameScript(stun);
                });
            }
            projectile.setToRemove();
        }

        public void OnUpdate(double diff)
        {
        }
    }
}