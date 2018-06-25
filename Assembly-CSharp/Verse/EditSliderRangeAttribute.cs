using System;

namespace Verse
{
	// Token: 0x02000E41 RID: 3649
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x040038F9 RID: 14585
		public float min = 0f;

		// Token: 0x040038FA RID: 14586
		public float max = 1f;

		// Token: 0x0600563F RID: 22079 RVA: 0x002C7BF3 File Offset: 0x002C5FF3
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
