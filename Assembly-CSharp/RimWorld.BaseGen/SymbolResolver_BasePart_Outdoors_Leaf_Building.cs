namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			return (byte)(base.CanResolve(rp) ? ((BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings) ? 1 : 0) : 0) != 0;
		}

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
