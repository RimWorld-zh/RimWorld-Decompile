using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200018B RID: 395
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x04000383 RID: 899
		public float distToHiveToAttack = 10f;

		// Token: 0x06000831 RID: 2097 RVA: 0x0004EECC File Offset: 0x0004D2CC
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
