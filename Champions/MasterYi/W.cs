using System.Numerics;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;

namespace Spells
{
    public class Meditate : GameScript
    {
        /*restoring 30/50/70/90/110 (+0.3) Health per second for 4 seconds. This healing is increased by 1% for every 1% of Master Yi's 
         missing Health.
        While channeling, Master Yi reduces incoming damage by 50/55/60/65/70%. This damage reduction is halved against turrets.*/
        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            var OriginalCoordsX = owner.X;
            var OriginalCoordsY = owner.Y;

            
           // for (float i = 0.0f; i < 4.0; i += 0.5f){   }
            for (int i = 0; i < 5; i++)
            {
                
                ApiFunctionManager.CreateTimer(i * 0.5f, () =>
                {
                    
                    ApiFunctionManager.AddParticleTarget(owner, "Meditate_eff.troy", owner, 1);
                    spell.spellAnimation("SPELL2", owner);
                    
                    

                    float healthPercentage = owner.GetStats().CurrentHealth / owner.GetStats().HealthPoints.Total;
                    //Example : 50/100= 0.5 
                    float missingHealthPercentage = 1.0f - healthPercentage;
                    //Right now the result would be 0.5 missingHP
                    //next we have to heal yi for 30/50/70/90/110 (+0.3) ap * 1% for every 1% missing hp
                    float ap = owner.GetStats().AbilityPower.Total * 0.3f;
                    float hptorecover = new float[] { 30f, 50f, 70f, 90f, 110f }[spell.Level - 1] + ap;
                    float bonushealth = hptorecover * missingHealthPercentage;

                    float ffs = (hptorecover + bonushealth);
                    float ffs2 = ffs / 4f;
                    owner.RestoreHealth(ffs2);
                });
                var buff = ((ObjAIBase)target).AddBuffGameScript("Meditate", "Meditate", spell, -1, true);

                ApiFunctionManager.CreateTimer(4.0f, () =>
                {
                    owner.RemoveBuffGameScript(buff);
                });
                if (OriginalCoordsX != owner.X || OriginalCoordsY != owner.Y)
                    //seems that this isnt working if anyone has an idea on how to break the healing once the player moves I'd apreciate
                {
                    break;
                }

            }
           




        }
        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
           
        }
        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
