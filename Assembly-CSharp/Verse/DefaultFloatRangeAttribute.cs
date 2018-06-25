using System;

namespace Verse
{
	// Token: 0x02000E44 RID: 3652
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06005643 RID: 22083 RVA: 0x002C7C81 File Offset: 0x002C6081
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
