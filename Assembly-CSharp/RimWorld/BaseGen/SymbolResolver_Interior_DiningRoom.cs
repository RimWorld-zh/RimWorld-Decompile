using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D9 RID: 985
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x060010EF RID: 4335 RVA: 0x000904D0 File Offset: 0x0008E8D0
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp);
			BaseGen.symbolStack.Push("randomlyPlaceMealsOnTables", rp);
			BaseGen.symbolStack.Push("placeChairsNearTables", rp);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGen.symbolStack.Push("thing", resolveParams);
			}
		}
	}
}
