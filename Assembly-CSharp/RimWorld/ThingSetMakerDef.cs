using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002DF RID: 735
	public class ThingSetMakerDef : Def
	{
		// Token: 0x04000790 RID: 1936
		public ThingSetMaker root;

		// Token: 0x04000791 RID: 1937
		public ThingSetMakerParams debugParams;

		// Token: 0x06000C1B RID: 3099 RVA: 0x0006B9A8 File Offset: 0x00069DA8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}
	}
}
