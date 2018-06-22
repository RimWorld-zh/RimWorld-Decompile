using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000B37 RID: 2871
	public class DutyDef : Def
	{
		// Token: 0x04002944 RID: 10564
		public ThinkNode thinkNode;

		// Token: 0x04002945 RID: 10565
		public ThinkNode constantThinkNode;

		// Token: 0x04002946 RID: 10566
		public bool alwaysShowWeapon = false;

		// Token: 0x04002947 RID: 10567
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x04002948 RID: 10568
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x04002949 RID: 10569
		public bool threatDisabled;
	}
}
