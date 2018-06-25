using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000398 RID: 920
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x00087084 File Offset: 0x00085484
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x000870CD File Offset: 0x000854CD
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp);
		}
	}
}
