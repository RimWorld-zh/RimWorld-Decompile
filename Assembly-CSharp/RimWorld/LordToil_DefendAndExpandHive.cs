using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200018B RID: 395
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x0004EEE4 File Offset: 0x0004D2E4
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

		// Token: 0x04000382 RID: 898
		public float distToHiveToAttack = 10f;
	}
}
