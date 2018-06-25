using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A0 RID: 928
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x06001027 RID: 4135 RVA: 0x00088270 File Offset: 0x00086670
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000882D4 File Offset: 0x000866D4
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams);
			BaseGen.globalSettings.basePart_buildingsResolved++;
		}
	}
}
