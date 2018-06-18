using System;

namespace Verse
{
	// Token: 0x02000E42 RID: 3650
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x0600561F RID: 22047 RVA: 0x002C5D79 File Offset: 0x002C4179
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
