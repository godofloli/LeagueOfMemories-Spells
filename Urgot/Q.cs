using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;

namespace Spells
{
    public class UrgotHeatseekingMissile : GameScript
    {
        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
        }

        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 1000;
            var trueCoords = current + range;
            spell.AddProjectile("UrgotHeatseekingLineMissile", trueCoords.X, trueCoords.Y);
        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
            var ad = owner.GetStats().AttackDamage.Total;
            var damage = new[] { 10 , 40 , 70 , 100 , 130 }[spell.Level - 1] + ad * 0.85f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            if (target.IsDead)
            {
                float manaToRecover = 20;
                var newMana = owner.GetStats().CurrentMana + manaToRecover;
                var maxMana = owner.GetStats().ManaPoints.Total;
                if (newMana >= maxMana)
                {
                    owner.GetStats().CurrentMana = maxMana;
                }
                else
                {
                    owner.GetStats().CurrentMana = newMana;
                }
            }
            projectile.setToRemove();
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
