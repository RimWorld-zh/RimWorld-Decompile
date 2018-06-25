using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B4B RID: 2891
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x040029D1 RID: 10705
		public bool isGameUniversal = false;

		// Token: 0x040029D2 RID: 10706
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040029D3 RID: 10707
		public bool selfConflicting = true;

		// Token: 0x06003F58 RID: 16216 RVA: 0x0021654C File Offset: 0x0021494C
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}
	}
}
