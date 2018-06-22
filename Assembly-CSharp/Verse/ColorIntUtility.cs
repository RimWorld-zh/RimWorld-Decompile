using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE4 RID: 3812
	public static class ColorIntUtility
	{
		// Token: 0x06005A81 RID: 23169 RVA: 0x002E71A0 File Offset: 0x002E55A0
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
