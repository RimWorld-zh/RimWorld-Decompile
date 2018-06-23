using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C8 RID: 968
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x04000A34 RID: 2612
		public string interior;

		// Token: 0x060010B5 RID: 4277 RVA: 0x0008E4BC File Offset: 0x0008C8BC
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
