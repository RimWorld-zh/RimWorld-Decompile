using RimWorld.BaseGen;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		private const int Radius = 3;

		public List<ItemCollectionGeneratorDef> itemCollectionGeneratorDefs;

		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordance.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 3).GetIterator();
			while (!iterator.Done())
			{
				if (iterator.Current.InBounds(map) && iterator.Current.GetEdifice(map) == null)
				{
					iterator.MoveNext();
					continue;
				}
				return false;
			}
			return true;
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 3).ClipInsideMap(map);
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
