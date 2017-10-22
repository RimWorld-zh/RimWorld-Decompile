using Verse;

namespace RimWorld
{
	public class Verb_BeatFire : Verb
	{
		private const int DamageAmount = 32;

		public Verb_BeatFire()
		{
			base.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		protected override bool TryCastShot()
		{
			Fire fire = (Fire)base.currentTarget.Thing;
			Pawn casterPawn = base.CasterPawn;
			bool result;
			if (casterPawn.stances.FullBodyBusy)
			{
				result = false;
			}
			else
			{
				Fire obj = fire;
				DamageDef extinguish = DamageDefOf.Extinguish;
				int amount = 32;
				Thing caster = base.caster;
				obj.TakeDamage(new DamageInfo(extinguish, amount, -1f, caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				casterPawn.Drawer.Notify_MeleeAttackOn(fire);
				result = true;
			}
			return result;
		}
	}
}
