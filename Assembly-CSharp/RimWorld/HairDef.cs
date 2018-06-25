using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029B RID: 667
	public class HairDef : Def
	{
		// Token: 0x04000606 RID: 1542
		[NoTranslate]
		public string texPath;

		// Token: 0x04000607 RID: 1543
		public HairGender hairGender = HairGender.Any;

		// Token: 0x04000608 RID: 1544
		[NoTranslate]
		public List<string> hairTags = new List<string>();
	}
}
