using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040C RID: 1036
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		// Token: 0x04000AD3 RID: 2771
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04000AD4 RID: 2772
		private const int Size = 7;

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x0009AB1C File Offset: 0x00098F1C
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0009AB38 File Offset: 0x00098F38
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
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
					if (!iterator.Current.InBounds(map) || iterator.Current.GetEdifice(map) != null)
					{
						return false;
					}
					iterator.MoveNext();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0009ABF0 File Offset: 0x00098FF0
		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			ItemStashContentsComp component = map.Parent.GetComponent<ItemStashContentsComp>();
			if (component != null && component.contents.Any)
			{
				resolveParams.stockpileConcreteContents = component.contents;
			}
			else
			{
				resolveParams.thingSetMakerDef = (this.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("storage", resolveParams);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
		}
	}
}
