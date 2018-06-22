using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D0 RID: 2512
	public class Verb_BeatFire : Verb
	{
		// Token: 0x0600385D RID: 14429 RVA: 0x001E1AC8 File Offset: 0x001DFEC8
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x001E1AE0 File Offset: 0x001DFEE0
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

		// Token: 0x04002407 RID: 9223
		private const int DamageAmount = 32;
	}
}
