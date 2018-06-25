using System;

namespace Verse
{
	// Token: 0x02000E4F RID: 3663
	public static class ModToolUtility
	{
		// Token: 0x06005666 RID: 22118 RVA: 0x002C8ACC File Offset: 0x002C6ECC
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
