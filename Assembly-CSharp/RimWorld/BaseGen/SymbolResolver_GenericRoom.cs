using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CA RID: 970
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x04000A34 RID: 2612
		public string interior;

		// Token: 0x060010B9 RID: 4281 RVA: 0x0008E60C File Offset: 0x0008CA0C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams);
			}
			BaseGen.symbolStack.Push("emptyRoom", rp);
		}
	}
}
