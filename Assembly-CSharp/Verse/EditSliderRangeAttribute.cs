using System;

namespace Verse
{
	// Token: 0x02000E3F RID: 3647
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x0600561B RID: 22043 RVA: 0x002C5CEA File Offset: 0x002C40EA
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040038E2 RID: 14562
		public float min = 0f;

		// Token: 0x040038E3 RID: 14563
		public float max = 1f;
	}
}
