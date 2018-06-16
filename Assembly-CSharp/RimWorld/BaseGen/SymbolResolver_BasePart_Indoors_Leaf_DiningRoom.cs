using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000396 RID: 918
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x06001004 RID: 4100 RVA: 0x00086D48 File Offset: 0x00085148
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00086D91 File Offset: 0x00085191
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp);
		}
	}
}
