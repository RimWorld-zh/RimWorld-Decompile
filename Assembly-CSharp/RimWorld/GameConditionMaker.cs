using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000308 RID: 776
	public static class GameConditionMaker
	{
		// Token: 0x06000D02 RID: 3330 RVA: 0x000715B0 File Offset: 0x0006F9B0
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1, -180000);
			gameCondition.Permanent = true;
			return gameCondition;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000715DC File Offset: 0x0006F9DC
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
