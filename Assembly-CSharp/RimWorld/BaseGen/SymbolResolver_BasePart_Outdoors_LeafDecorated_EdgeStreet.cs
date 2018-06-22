using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039C RID: 924
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_EdgeStreet : SymbolResolver
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x00087F88 File Offset: 0x00086388
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
