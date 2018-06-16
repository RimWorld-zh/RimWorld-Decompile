using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02000398 RID: 920
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x00086E10 File Offset: 0x00085210
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_indoors_division", rp);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_indoors_leaf", rp);
			}
		}
	}
}
