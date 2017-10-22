using RimWorld.BaseGen;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		public List<ItemCollectionGeneratorDef> itemCollectionGeneratorDefs;

		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);

		private const int Size = 7;

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else if (!c.SupportsStructureType(map, TerrainAffordance.Heavy))
			{
				result = false;
			}
			else if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				result = false;
			}
			else
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 7, 7).GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.InBounds(map) && iterator.Current.GetEdifice(map) == null)
					{
						iterator.MoveNext();
						continue;
					}
					goto IL_0084;
				}
				result = true;
			}
			goto IL_00a6;
			IL_0084:
			result = false;
			goto IL_00a6;
			IL_00a6:
			return result;
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			ResolveParams resolveParams = new ResolveParams
			{
				rect = cellRect,
				faction = map.ParentFaction
			};
			ItemStashContentsComp component = ((WorldObject)map.info.parent).GetComponent<ItemStashContentsComp>();
			if (component != null && component.contents.Any)
			{
				resolveParams.stockpileConcreteContents = component.contents;
			}
			else
			{
				resolveParams.stockpileMarketValue = new float?(this.totalValueRange.RandomInRange);
				if (this.itemCollectionGeneratorDefs != null)
				{
					resolveParams.itemCollectionGeneratorDef = this.itemCollectionGeneratorDefs.RandomElement();
				}
			}
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("storage", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
			MapGenerator.SetVar("RectOfInterest", cellRect);
		}
	}
}
