using System;

namespace Verse
{
	// Token: 0x02000E40 RID: 3648
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x040038F1 RID: 14577
		public float min = 0f;

		// Token: 0x040038F2 RID: 14578
		public float max = 1f;

		// Token: 0x0600563F RID: 22079 RVA: 0x002C7A07 File Offset: 0x002C5E07
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
