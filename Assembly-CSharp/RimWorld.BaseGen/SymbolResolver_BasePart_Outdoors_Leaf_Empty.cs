namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			return (byte)(base.CanResolve(rp) ? ((BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings) ? 1 : 0) : 0) != 0;
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
