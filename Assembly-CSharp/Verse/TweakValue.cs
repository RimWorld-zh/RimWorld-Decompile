using System;

namespace Verse
{
	// Token: 0x02000E0F RID: 3599
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x06005180 RID: 20864 RVA: 0x0029C51D File Offset: 0x0029A91D
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04003564 RID: 13668
		public string category;

		// Token: 0x04003565 RID: 13669
		public float min;

		// Token: 0x04003566 RID: 13670
		public float max;
	}
}
