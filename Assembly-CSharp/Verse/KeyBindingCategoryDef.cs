using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B4C RID: 2892
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x040029D8 RID: 10712
		public bool isGameUniversal = false;

		// Token: 0x040029D9 RID: 10713
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040029DA RID: 10714
		public bool selfConflicting = true;

		// Token: 0x06003F58 RID: 16216 RVA: 0x0021682C File Offset: 0x00214C2C
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}
	}
}
