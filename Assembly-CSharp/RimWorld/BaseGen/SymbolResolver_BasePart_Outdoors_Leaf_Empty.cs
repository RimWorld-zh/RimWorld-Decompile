using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A1 RID: 929
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x00088344 File Offset: 0x00086744
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0008838D File Offset: 0x0008678D
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
