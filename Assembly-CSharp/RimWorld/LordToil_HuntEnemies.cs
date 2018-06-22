using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000193 RID: 403
	public class LordToil_HuntEnemies : LordToil
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x0004FCFE File Offset: 0x0004E0FE
		public LordToil_HuntEnemies(IntVec3 fallbackLocation)
		{
			this.data = new LordToilData_HuntEnemies();
			this.Data.fallbackLocation = fallbackLocation;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0004FD20 File Offset: 0x0004E120
		private LordToilData_HuntEnemies Data
		{
			get
			{
				return (LordToilData_HuntEnemies)this.data;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0004FD40 File Offset: 0x0004E140
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0004FD58 File Offset: 0x0004E158
		public override void UpdateAllDuties()
		{
			LordToilData_HuntEnemies data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!data.fallbackLocation.IsValid)
				{
					RCellFinder.TryFindRandomSpotJustOutsideColony(this.lord.ownedPawns[0], out data.fallbackLocation);
				}
				pawn.mindState.duty = new PawnDuty(DutyDefOf.HuntEnemiesIndividual);
				pawn.mindState.duty.focusSecond = data.fallbackLocation;
			}
		}
	}
}
