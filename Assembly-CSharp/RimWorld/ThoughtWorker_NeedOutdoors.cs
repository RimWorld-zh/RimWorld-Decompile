using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000232 RID: 562
	public class ThoughtWorker_NeedOutdoors : ThoughtWorker
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x00059FA4 File Offset: 0x000583A4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.outdoors == null)
			{
				result = ThoughtState.Inactive;
			}
			else if (p.HostFaction != null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.outdoors.CurCategory)
				{
				case OutdoorsCategory.Entombed:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case OutdoorsCategory.Trapped:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case OutdoorsCategory.CabinFeverSevere:
					result = ThoughtState.ActiveAtStage(2);
					break;
				case OutdoorsCategory.CabinFeverLight:
					result = ThoughtState.ActiveAtStage(3);
					break;
				case OutdoorsCategory.NeedFreshAir:
					result = ThoughtState.ActiveAtStage(4);
					break;
				case OutdoorsCategory.Free:
					result = ThoughtState.Inactive;
					break;
				default:
					throw new InvalidOperationException("Unknown OutdoorsCategory");
				}
			}
			return result;
		}
	}
}
