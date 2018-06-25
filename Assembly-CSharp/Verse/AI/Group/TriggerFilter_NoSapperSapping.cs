using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A0D RID: 2573
	public class TriggerFilter_NoSapperSapping : TriggerFilter
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x001E821C File Offset: 0x001E661C
		public override bool AllowActivation(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if ((pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Sapper && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Mine && pawn.CurJob.targetA.Cell.InHorDistOf(pawn.Position, 5f)) || (pawn.CurJob.def == JobDefOf.UseVerbOnThing && pawn.CurJob.targetA.Cell.InHorDistOf(pawn.Position, 20f)))
				{
					return false;
				}
			}
			return true;
		}
	}
}
