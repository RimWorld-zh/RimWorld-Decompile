using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022F RID: 559
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x00059D74 File Offset: 0x00058174
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
