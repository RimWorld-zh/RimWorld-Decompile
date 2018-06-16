using System;

namespace Verse
{
	// Token: 0x02000E40 RID: 3648
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x0600561D RID: 22045 RVA: 0x002C5CEA File Offset: 0x002C40EA
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040038E4 RID: 14564
		public float min = 0f;

		// Token: 0x040038E5 RID: 14565
		public float max = 1f;
	}
}
