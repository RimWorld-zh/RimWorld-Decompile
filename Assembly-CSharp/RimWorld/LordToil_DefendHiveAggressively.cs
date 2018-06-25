using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200018D RID: 397
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x04000385 RID: 901
		public float distToHiveToAttack = 40f;

		// Token: 0x06000836 RID: 2102 RVA: 0x0004EFF8 File Offset: 0x0004D3F8
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
