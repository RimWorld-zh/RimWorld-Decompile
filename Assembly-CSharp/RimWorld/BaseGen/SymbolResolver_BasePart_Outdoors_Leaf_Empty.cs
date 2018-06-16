using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039F RID: 927
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x06001027 RID: 4135 RVA: 0x00088008 File Offset: 0x00086408
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00088051 File Offset: 0x00086451
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
