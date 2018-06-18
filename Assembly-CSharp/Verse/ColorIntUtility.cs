using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE5 RID: 3813
	public static class ColorIntUtility
	{
		// Token: 0x06005A60 RID: 23136 RVA: 0x002E538C File Offset: 0x002E378C
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
