using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;

namespace Spells
{
    public class IreliaGatotsu : GameScript
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
            var range = to * 650;
            var trueCoords = current + range;
            spell.AddProjectileTarget("IreliaGatotsu", target, true);
        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
            float ad = owner.GetStats().AttackDamage.Total;
            float damage = new[] { 20, 50, 80, 110, 140 }[spell.Level - 1] + ad * 1.2f;
            spell.Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL,
                false);

            var current = new Vector2(target.X, target.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 50;
            var trueCoords = current + range;
            ApiFunctionManager.DashToLocation((ObjAIBase)spell.Owner, trueCoords.X, trueCoords.Y, spell.SpellData.MissileSpeed, true);
            if (target.IsDead)
            {
                spell.LowerCooldown(0, spell.getCooldown());
                float manaToRecover = 35;
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