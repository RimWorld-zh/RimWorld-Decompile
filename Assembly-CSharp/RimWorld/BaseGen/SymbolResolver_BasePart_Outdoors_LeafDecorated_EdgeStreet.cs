using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039E RID: 926
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_EdgeStreet : SymbolResolver
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x000880D8 File Offset: 0x000864D8
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			BaseGen.symbolStack.Push("edgeStreet", resolveParams);
			ResolveParams resolveParams2 = rp;
			resolveParams2.rect = rp.rect.ContractedBy(1);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams2);
		}
	}
}
