using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D4 RID: 2516
	public class Verb_BeatFire : Verb
	{
		// Token: 0x06003863 RID: 14435 RVA: 0x001E18F0 File Offset: 0x001DFCF0
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x001E1908 File Offset: 0x001DFD08
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = base.CasterPawn;
			bool result;
			if (casterPawn.stances.FullBodyBusy)
			{
				result = false;
			}
			else
			{
				Thing thing = fire;
				DamageDef extinguish = DamageDefOf.Extinguish;
				float amount = 32f;
				Thing caster = this.caster;
				thing.TakeDamage(new DamageInfo(extinguish, amount, -1f, caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				casterPawn.Drawer.Notify_MeleeAttackOn(fire);
				result = true;
			}
			return result;
		}

		// Token: 0x0400240C RID: 9228
		private const int DamageAmount = 32;
	}
}
