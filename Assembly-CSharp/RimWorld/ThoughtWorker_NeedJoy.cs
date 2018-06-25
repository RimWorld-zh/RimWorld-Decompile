using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000230 RID: 560
	public class ThoughtWorker_NeedJoy : ThoughtWorker
	{
		// Token: 0x06000A2E RID: 2606 RVA: 0x00059E08 File Offset: 0x00058208
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.joy == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.joy.CurCategory)
				{
				case JoyCategory.Empty:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case JoyCategory.VeryLow:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case JoyCategory.Low:
					result = ThoughtState.ActiveAtStage(2);
					break;
				case JoyCategory.Satisfied:
					result = ThoughtState.Inactive;
					break;
				case JoyCategory.High:
					result = ThoughtState.ActiveAtStage(3);
					break;
				case JoyCategory.Extreme:
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
