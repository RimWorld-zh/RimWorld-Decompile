using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E1 RID: 737
	public class ThingSetMakerDef : Def
	{
		// Token: 0x04000793 RID: 1939
		public ThingSetMaker root;

		// Token: 0x04000794 RID: 1940
		public ThingSetMakerParams debugParams;

		// Token: 0x06000C1E RID: 3102 RVA: 0x0006BB00 File Offset: 0x00069F00
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}
	}
}
