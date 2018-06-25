using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B8 RID: 952
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x0008B7A8 File Offset: 0x00089BA8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
				RoofDef def = rp.roofDef ?? RoofDefOf.RoofConstructed;
				CellRect.CellRectIterator iterator = rp.rect.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					if (!roofGrid.Roofed(c))
					{
						roofGrid.SetRoof(c, def);
					}
					iterator.MoveNext();
				}
			}
		}
	}
}
