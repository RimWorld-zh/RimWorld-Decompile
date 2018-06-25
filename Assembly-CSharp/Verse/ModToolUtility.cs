using System;

namespace Verse
{
	// Token: 0x02000E50 RID: 3664
	public static class ModToolUtility
	{
		// Token: 0x06005666 RID: 22118 RVA: 0x002C8CB8 File Offset: 0x002C70B8
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
