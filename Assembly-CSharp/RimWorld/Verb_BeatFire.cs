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
			if (casterPawn.stances.FullBodyBusy)
			{
				return false;
			}
			Fire obj = fire;
			Thing caster = base.caster;
			obj.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32, -1f, caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}
	}
}
