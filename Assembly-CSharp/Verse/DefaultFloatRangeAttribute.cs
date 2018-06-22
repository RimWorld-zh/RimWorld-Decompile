using System;

namespace Verse
{
	// Token: 0x02000E41 RID: 3649
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x0600563F RID: 22079 RVA: 0x002C7969 File Offset: 0x002C5D69
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
