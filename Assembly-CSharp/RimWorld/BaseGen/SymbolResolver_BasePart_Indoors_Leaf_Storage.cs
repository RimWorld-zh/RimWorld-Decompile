using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000397 RID: 919
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		// Token: 0x06001007 RID: 4103 RVA: 0x00086F98 File Offset: 0x00085398
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00086FE1 File Offset: 0x000853E1
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp);
		}
	}
}
