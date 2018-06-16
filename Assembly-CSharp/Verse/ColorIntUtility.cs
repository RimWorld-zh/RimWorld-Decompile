using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE6 RID: 3814
	public static class ColorIntUtility
	{
		// Token: 0x06005A62 RID: 23138 RVA: 0x002E52B4 File Offset: 0x002E36B4
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
