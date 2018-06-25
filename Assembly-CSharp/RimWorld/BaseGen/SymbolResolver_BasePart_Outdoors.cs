using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A5 RID: 933
	public class SymbolResolver_BasePart_Outdoors : SymbolResolver
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x0008885C File Offset: 0x00086C5C
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 23 || rp.rect.Height > 23 || ((rp.rect.Width >= 11 || rp.rect.Height >= 11) && Rand.Bool);
			ResolveParams resolveParams = rp;
			resolveParams.pathwayFloorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_outdoors_division", resolveParams);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafPossiblyDecorated", resolveParams);
			}
		}
	}
}
