using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FD5 RID: 4053
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x0600620A RID: 25098 RVA: 0x001E317C File Offset: 0x001E157C
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x001E319C File Offset: 0x001E159C
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

		// Token: 0x0600620C RID: 25100 RVA: 0x001E3244 File Offset: 0x001E1644
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
