using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A0 RID: 928
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x00088260 File Offset: 0x00086660
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x000882C4 File Offset: 0x000866C4
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
