using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030A RID: 778
	public static class GameConditionMaker
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00071708 File Offset: 0x0006FB08
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1, -180000);
			gameCondition.Permanent = true;
			return gameCondition;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00071734 File Offset: 0x0006FB34
		public static GameCondition MakeCondition(GameConditionDef def, int duration = -1, int startTickOffset = 0)
		{
			GameCondition gameCondition = (GameCondition)Activator.CreateInstance(def.conditionClass);
			gameCondition.startTick = Find.TickManager.TicksGame + startTickOffset;
			gameCondition.def = def;
			gameCondition.Duration = duration;
			return gameCondition;
		}
	}
}
