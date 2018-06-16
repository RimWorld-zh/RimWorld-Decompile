using System;

namespace Verse
{
	// Token: 0x02000E43 RID: 3651
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06005621 RID: 22049 RVA: 0x002C5D79 File Offset: 0x002C4179
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
