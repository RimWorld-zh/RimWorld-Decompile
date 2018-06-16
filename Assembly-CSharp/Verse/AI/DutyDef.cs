using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000B3B RID: 2875
	public class DutyDef : Def
	{
		// Token: 0x04002947 RID: 10567
		public ThinkNode thinkNode;

		// Token: 0x04002948 RID: 10568
		public ThinkNode constantThinkNode;

		// Token: 0x04002949 RID: 10569
		public bool alwaysShowWeapon = false;

		// Token: 0x0400294A RID: 10570
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x0400294B RID: 10571
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x0400294C RID: 10572
		public bool threatDisabled;
	}
}
