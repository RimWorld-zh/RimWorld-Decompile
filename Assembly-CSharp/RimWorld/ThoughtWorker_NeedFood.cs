using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022E RID: 558
	public class ThoughtWorker_NeedFood : ThoughtWorker
	{
		// Token: 0x06000A29 RID: 2601 RVA: 0x00059CB0 File Offset: 0x000580B0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.food == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.food.CurCategory)
				{
				case HungerCategory.Fed:
					result = ThoughtState.Inactive;
					break;
				case HungerCategory.Hungry:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case HungerCategory.UrgentlyHungry:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case HungerCategory.Starving:
				{
					Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					int num = (firstHediffOfDef != null) ? firstHediffOfDef.CurStageIndex : 0;
					result = ThoughtState.ActiveAtStage(2 + num);
					break;
				}
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
