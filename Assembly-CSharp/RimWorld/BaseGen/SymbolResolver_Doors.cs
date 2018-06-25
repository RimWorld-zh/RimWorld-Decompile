using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AA RID: 938
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x04000A16 RID: 2582
		private const float ExtraDoorChance = 0.25f;

		// Token: 0x06001046 RID: 4166 RVA: 0x0008911C File Offset: 0x0008751C
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp);
		}
	}
}
