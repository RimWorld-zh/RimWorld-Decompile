using System;

namespace Verse
{
	// Token: 0x02000E0E RID: 3598
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x0600517E RID: 20862 RVA: 0x0029C4FD File Offset: 0x0029A8FD
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04003562 RID: 13666
		public string category;

		// Token: 0x04003563 RID: 13667
		public float min;

		// Token: 0x04003564 RID: 13668
		public float max;
	}
}
