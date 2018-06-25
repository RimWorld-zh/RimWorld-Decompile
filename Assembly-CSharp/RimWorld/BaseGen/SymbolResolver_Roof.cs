using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Roof : SymbolResolver
	{
		public SymbolResolver_Roof()
		{
		}

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
