using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AE RID: 430
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x060008DD RID: 2269 RVA: 0x00053871 File Offset: 0x00051C71
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00053888 File Offset: 0x00051C88
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000538A8 File Offset: 0x00051CA8
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

		// Token: 0x040003BF RID: 959
		private const int CheckInterval = 120;

		// Token: 0x040003C0 RID: 960
		private const int MinTicksSinceDamage = 300;
	}
}
