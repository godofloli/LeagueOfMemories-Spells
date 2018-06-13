using System.Collections.Generic;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;


namespace Spells
{
    public class Highlander : GameScript
    {

        public void OnActivate(Champion owner)
        {

        }

        public void OnDeactivate(Champion owner)
        {
        }
        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            spell.spellAnimation("SPELL4", owner);
            ApiFunctionManager.AddBuffHUDVisual("Highlander", 10.0f, 1, owner, 10.0f);
        }
        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            /*Passive: Champion kills and assists reduce the remaining cooldown of Master Yi's basic 
             * abilities by 70%.

            Active: Increases Movement Speed by 25/35/45%, Attack Speed by 30/55/80%, and grants
            immunity to all slowing effects for 10 seconds. While active, champion kills and assists
            extend the duration of Highlander by 4 seconds.*/
            float duration =  10.0f;

            var highlanderbuff = ((ObjAIBase)target).AddBuffGameScript("HighlanderBuff", "HighlanderBuff", spell, -1, true);

            ApiFunctionManager.CreateTimer(duration, () =>
            {
                owner.RemoveBuffGameScript(highlanderbuff);
            });

        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
