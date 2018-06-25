using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AE RID: 430
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x040003BE RID: 958
		private const int CheckInterval = 120;

		// Token: 0x040003BF RID: 959
		private const int MinTicksSinceDamage = 300;

		// Token: 0x060008DA RID: 2266 RVA: 0x00053881 File Offset: 0x00051C81
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00053898 File Offset: 0x00051C98
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000538B8 File Offset: 0x00051CB8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (this.data == null || !(this.data is TriggerData_PawnCycleInd))
				{
					BackCompatibility.TriggerDataPawnCycleIndNull(this);
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					TriggerData_PawnCycleInd data = this.Data;
					data.pawnCycleInd++;
					if (data.pawnCycleInd >= lord.ownedPawns.Count)
					{
						data.pawnCycleInd = 0;
					}
					if (lord.ownedPawns.Any<Pawn>())
					{
						Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
						if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null)
						{
							Pawn pawn2;
							if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, null) && !GenAI.InDangerousCombat(pawn))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
	}
}
