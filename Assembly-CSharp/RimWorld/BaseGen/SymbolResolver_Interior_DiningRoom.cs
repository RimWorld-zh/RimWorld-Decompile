using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DB RID: 987
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x0009081C File Offset: 0x0008EC1C
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
