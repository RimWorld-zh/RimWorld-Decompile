using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02000394 RID: 916
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		// Token: 0x040009F3 RID: 2547
		private const int MinLengthAfterSplit = 5;

		// Token: 0x040009F4 RID: 2548
		private const int MinWidthOrHeight = 9;

		// Token: 0x06000FFD RID: 4093 RVA: 0x00086C04 File Offset: 0x00085004
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.rect.Width >= 9 || rp.rect.Height >= 9);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00086C54 File Offset: 0x00085054
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width < 9 && rp.rect.Height < 9)
			{
				Log.Warning("Too small rect. params=" + rp, false);
			}
			else
			{
				bool flag = (Rand.Bool && rp.rect.Height >= 9) || rp.rect.Width < 9;
				if (flag)
				{
					int num = Rand.RangeInclusive(4, rp.rect.Height - 5);
					ResolveParams resolveParams = rp;
					resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num + 1);
					BaseGen.symbolStack.Push("basePart_indoors", resolveParams);
					ResolveParams resolveParams2 = rp;
					resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num, rp.rect.Width, rp.rect.Height - num);
					BaseGen.symbolStack.Push("basePart_indoors", resolveParams2);
				}
				else
				{
					int num2 = Rand.RangeInclusive(4, rp.rect.Width - 5);
					ResolveParams resolveParams3 = rp;
					resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num2 + 1, rp.rect.Height);
					BaseGen.symbolStack.Push("basePart_indoors", resolveParams3);
					ResolveParams resolveParams4 = rp;
					resolveParams4.rect = new CellRect(rp.rect.minX + num2, rp.rect.minZ, rp.rect.Width - num2, rp.rect.Height);
					BaseGen.symbolStack.Push("basePart_indoors", resolveParams4);
				}
			}
		}
	}
}
