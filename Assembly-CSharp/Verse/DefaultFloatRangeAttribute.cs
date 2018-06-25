using System;

namespace Verse
{
	// Token: 0x02000E43 RID: 3651
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06005643 RID: 22083 RVA: 0x002C7A95 File Offset: 0x002C5E95
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
