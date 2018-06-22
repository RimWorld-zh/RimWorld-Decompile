using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FD5 RID: 4053
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06006233 RID: 25139 RVA: 0x001E3490 File Offset: 0x001E1890
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x001E34B0 File Offset: 0x001E18B0
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

		// Token: 0x06006235 RID: 25141 RVA: 0x001E3558 File Offset: 0x001E1958
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
