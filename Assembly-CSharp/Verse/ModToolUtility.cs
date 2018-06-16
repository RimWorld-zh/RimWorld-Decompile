using System;

namespace Verse
{
	// Token: 0x02000E4F RID: 3663
	public static class ModToolUtility
	{
		// Token: 0x06005644 RID: 22084 RVA: 0x002C6DB0 File Offset: 0x002C51B0
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
