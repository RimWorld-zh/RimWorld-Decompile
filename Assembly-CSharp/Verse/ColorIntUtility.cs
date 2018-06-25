using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE6 RID: 3814
	public static class ColorIntUtility
	{
		// Token: 0x06005A84 RID: 23172 RVA: 0x002E72C0 File Offset: 0x002E56C0
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
