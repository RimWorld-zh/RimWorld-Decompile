using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C8 RID: 968
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x060010B5 RID: 4277 RVA: 0x0008E2D0 File Offset: 0x0008C6D0
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

		// Token: 0x04000A32 RID: 2610
		public string interior;
	}
}
