using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A0A RID: 2570
	public class TriggerFilter_NoSapperSapping : TriggerFilter
	{
		// Token: 0x0600398F RID: 14735 RVA: 0x001E7DC4 File Offset: 0x001E61C4
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
