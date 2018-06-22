using System;

namespace Verse
{
	// Token: 0x02000E3E RID: 3646
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x0600563B RID: 22075 RVA: 0x002C78DB File Offset: 0x002C5CDB
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040038F1 RID: 14577
		public float min = 0f;

		// Token: 0x040038F2 RID: 14578
		public float max = 1f;
	}
}
