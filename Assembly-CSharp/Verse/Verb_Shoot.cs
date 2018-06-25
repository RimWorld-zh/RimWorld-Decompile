using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FDA RID: 4058
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06006242 RID: 25154 RVA: 0x001E3890 File Offset: 0x001E1C90
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x06006243 RID: 25155 RVA: 0x001E38B0 File Offset: 0x001E1CB0
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

		// Token: 0x06006244 RID: 25156 RVA: 0x001E3958 File Offset: 0x001E1D58
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
