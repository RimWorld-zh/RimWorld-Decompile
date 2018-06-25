using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F1 RID: 497
	public class ThoughtWorker_PrisonCellImpressiveness : ThoughtWorker_RoomImpressiveness
	{
		// Token: 0x060009AA RID: 2474 RVA: 0x00057384 File Offset: 0x00055784
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.IsPrisoner)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				ThoughtState thoughtState = base.CurrentStateInternal(p);
				if (thoughtState.Active && p.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.PrisonCell)
				{
					result = thoughtState;
				}
				else
				{
					result = ThoughtState.Inactive;
				}
			}
			return result;
		}
	}
}
