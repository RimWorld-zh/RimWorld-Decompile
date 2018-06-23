using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B49 RID: 2889
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x040029D1 RID: 10705
		public bool isGameUniversal = false;

		// Token: 0x040029D2 RID: 10706
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040029D3 RID: 10707
		public bool selfConflicting = true;

		// Token: 0x06003F55 RID: 16213 RVA: 0x00216470 File Offset: 0x00214870
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}
	}
}
