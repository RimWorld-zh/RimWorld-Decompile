using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E1 RID: 737
	public class ThingSetMakerDef : Def
	{
		// Token: 0x04000790 RID: 1936
		public ThingSetMaker root;

		// Token: 0x04000791 RID: 1937
		public ThingSetMakerParams debugParams;

		// Token: 0x06000C1F RID: 3103 RVA: 0x0006BAF8 File Offset: 0x00069EF8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}
	}
}
