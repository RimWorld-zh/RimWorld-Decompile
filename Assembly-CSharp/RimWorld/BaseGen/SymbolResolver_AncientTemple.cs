using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C1 RID: 961
	public class SymbolResolver_AncientTemple : SymbolResolver
	{
		// Token: 0x0600109E RID: 4254 RVA: 0x0008CE80 File Offset: 0x0008B280
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("ensureCanHoldRoof", rp);
			ResolveParams resolveParams = rp;
			resolveParams.rect = rp.rect.ContractedBy(1);
			BaseGen.symbolStack.Push("interior_ancientTemple", resolveParams);
			ResolveParams resolveParams2 = rp;
			resolveParams2.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			bool? clearEdificeOnly = rp.clearEdificeOnly;
			resolveParams2.clearEdificeOnly = new bool?(clearEdificeOnly == null || clearEdificeOnly.Value);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams2);
		}
	}
}
