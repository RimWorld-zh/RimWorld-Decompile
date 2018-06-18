using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FD4 RID: 4052
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06006208 RID: 25096 RVA: 0x001E3250 File Offset: 0x001E1650
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x001E3270 File Offset: 0x001E1670
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && base.CasterIsPawn && base.CasterPawn.skills != null)
			{
				float num = (!pawn.HostileTo(this.caster)) ? 20f : 170f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, base.CasterPawn, this.ownerEquipment);
				base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
			}
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x001E3318 File Offset: 0x001E1718
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && base.CasterIsPawn)
			{
				base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}
