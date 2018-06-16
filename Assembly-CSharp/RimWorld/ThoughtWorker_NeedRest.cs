using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022D RID: 557
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06000A2A RID: 2602 RVA: 0x00059BE0 File Offset: 0x00057FE0
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
