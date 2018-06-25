using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000231 RID: 561
	public class ThoughtWorker_NeedComfort : ThoughtWorker
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x00059EB8 File Offset: 0x000582B8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.comfort == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.comfort.CurCategory)
				{
				case ComfortCategory.Uncomfortable:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case ComfortCategory.Normal:
					result = ThoughtState.Inactive;
					break;
				case ComfortCategory.Comfortable:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case ComfortCategory.VeryComfortable:
					result = ThoughtState.ActiveAtStage(2);
					break;
				case ComfortCategory.ExtremelyComfortable:
					result = ThoughtState.ActiveAtStage(3);
					break;
				case ComfortCategory.LuxuriantlyComfortable:
					result = ThoughtState.ActiveAtStage(4);
					break;
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
