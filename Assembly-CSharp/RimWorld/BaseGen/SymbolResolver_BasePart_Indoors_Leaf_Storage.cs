using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000399 RID: 921
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x000870F8 File Offset: 0x000854F8
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00087141 File Offset: 0x00085541
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp);
		}
	}
}
