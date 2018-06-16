using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BF RID: 959
	public class SymbolResolver_AncientTemple : SymbolResolver
	{
		// Token: 0x0600109B RID: 4251 RVA: 0x0008CB34 File Offset: 0x0008AF34
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
