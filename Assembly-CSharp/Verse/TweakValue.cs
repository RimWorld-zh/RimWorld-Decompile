using System;

namespace Verse
{
	// Token: 0x02000E0E RID: 3598
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x04003570 RID: 13680
		public string category;

		// Token: 0x04003571 RID: 13681
		public float min;

		// Token: 0x04003572 RID: 13682
		public float max;

		// Token: 0x06005196 RID: 20886 RVA: 0x0029DEE9 File Offset: 0x0029C2E9
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}
	}
}
