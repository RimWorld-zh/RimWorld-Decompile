using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000398 RID: 920
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x06001007 RID: 4103 RVA: 0x00087094 File Offset: 0x00085494
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000870DD File Offset: 0x000854DD
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp);
		}
	}
}
