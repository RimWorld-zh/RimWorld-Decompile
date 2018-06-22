using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022D RID: 557
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x00059C24 File Offset: 0x00058024
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.rest == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.rest.CurCategory)
				{
				case RestCategory.Rested:
					result = ThoughtState.Inactive;
					break;
				case RestCategory.Tired:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case RestCategory.VeryTired:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case RestCategory.Exhausted:
					result = ThoughtState.ActiveAtStage(2);
					break;
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
