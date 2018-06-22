using System;

namespace Verse
{
	// Token: 0x02000E4D RID: 3661
	public static class ModToolUtility
	{
		// Token: 0x06005662 RID: 22114 RVA: 0x002C89A0 File Offset: 0x002C6DA0
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
