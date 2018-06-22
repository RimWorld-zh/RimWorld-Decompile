using System;

namespace Verse
{
	// Token: 0x02000E0B RID: 3595
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x06005192 RID: 20882 RVA: 0x0029DADD File Offset: 0x0029BEDD
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04003569 RID: 13673
		public string category;

		// Token: 0x0400356A RID: 13674
		public float min;

		// Token: 0x0400356B RID: 13675
		public float max;
	}
}
