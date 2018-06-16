using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B4D RID: 2893
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06003F53 RID: 16211 RVA: 0x00215D70 File Offset: 0x00214170
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x040029D3 RID: 10707
		public bool isGameUniversal = false;

		// Token: 0x040029D4 RID: 10708
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x040029D5 RID: 10709
		public bool selfConflicting = true;
	}
}
