using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DF RID: 735
	public class ThingSetMakerDef : Def
	{
		// Token: 0x06000C1D RID: 3101 RVA: 0x0006B940 File Offset: 0x00069D40
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x04000791 RID: 1937
		public ThingSetMaker root;

		// Token: 0x04000792 RID: 1938
		public ThingSetMakerParams debugParams;
	}
}
