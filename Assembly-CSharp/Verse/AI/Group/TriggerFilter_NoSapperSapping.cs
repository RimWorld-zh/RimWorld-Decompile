using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A0E RID: 2574
	public class TriggerFilter_NoSapperSapping : TriggerFilter
	{
		// Token: 0x06003993 RID: 14739 RVA: 0x001E7AB0 File Offset: 0x001E5EB0
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
