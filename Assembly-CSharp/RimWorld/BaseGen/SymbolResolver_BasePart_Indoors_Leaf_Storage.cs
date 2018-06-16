using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000397 RID: 919
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		// Token: 0x06001007 RID: 4103 RVA: 0x00086DAC File Offset: 0x000851AC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00086DF5 File Offset: 0x000851F5
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp);
		}
	}
}
