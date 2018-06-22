using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039E RID: 926
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x00088110 File Offset: 0x00086510
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00088174 File Offset: 0x00086574
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
