using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000299 RID: 665
	public class HairDef : Def
	{
		// Token: 0x04000605 RID: 1541
		[NoTranslate]
		public string texPath;

		// Token: 0x04000606 RID: 1542
		public HairGender hairGender = HairGender.Any;

		// Token: 0x04000607 RID: 1543
		[NoTranslate]
		public List<string> hairTags = new List<string>();
	}
}
