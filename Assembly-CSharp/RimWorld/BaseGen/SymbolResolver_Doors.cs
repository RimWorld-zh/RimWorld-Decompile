using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A8 RID: 936
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x04000A13 RID: 2579
		private const float ExtraDoorChance = 0.25f;

		// Token: 0x06001043 RID: 4163 RVA: 0x00088FBC File Offset: 0x000873BC
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
