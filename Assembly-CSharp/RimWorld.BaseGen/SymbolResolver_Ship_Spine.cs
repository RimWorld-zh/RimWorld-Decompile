using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Ship_Spine : SymbolResolver
	{
		public override void Resolve(ResolveParams rp)
		{
			ThingDef ship_Beam = ThingDefOf.Ship_Beam;
			Map map = BaseGen.globalSettings.map;
			if (!rp.thingRot.HasValue && !rp.thrustAxis.HasValue)
			{
				rp.thrustAxis = new Rot4?(Rot4.Random);
				rp.thingRot = rp.thrustAxis;
			}
			IntVec2 intVec = IntVec2.Invalid;
			IntVec2 b = IntVec2.Invalid;
			IntVec2 toIntVec = rp.thingRot.Value.FacingCell.ToIntVec2;
			int num = 0;
			while (true)
			{
				if (rp.thingRot.Value.IsHorizontal)
				{
					int newZ = Rand.Range(rp.rect.minZ + 1, rp.rect.maxZ - 2);
					intVec = new IntVec2((!(rp.thingRot.Value == Rot4.East)) ? rp.rect.maxX : rp.rect.minX, newZ);
					b = new IntVec2((!(rp.thingRot.Value == Rot4.East)) ? rp.rect.minX : rp.rect.maxX, newZ);
				}
				else
				{
					int newX = Rand.Range(rp.rect.minX + 1, rp.rect.maxX - 2);
					intVec = new IntVec2(newX, (!(rp.thingRot.Value == Rot4.North)) ? rp.rect.maxZ : rp.rect.minZ);
					b = new IntVec2(newX, (!(rp.thingRot.Value == Rot4.North)) ? rp.rect.minZ : rp.rect.maxZ);
				}
				bool? allowPlacementOffEdge = rp.allowPlacementOffEdge;
				if (allowPlacementOffEdge.HasValue && !allowPlacementOffEdge.Value && !(intVec - toIntVec).ToIntVec3.GetThingList(map).Any((Predicate<Thing>)((Thing thing) => thing.def == ThingDefOf.Ship_Beam)))
				{
					if (num != 20)
					{
						num++;
						continue;
					}
					return;
				}
				break;
			}
			int magnitudeManhattan = (intVec - b).MagnitudeManhattan;
			float magnitude = (intVec - b).Magnitude;
			IntVec2 size = ship_Beam.Size;
			if (!(magnitude < (float)size.z))
			{
				int num2;
				int num5;
				while (true)
				{
					bool? allowPlacementOffEdge2 = rp.allowPlacementOffEdge;
					num2 = ((!allowPlacementOffEdge2.HasValue || allowPlacementOffEdge2.Value) ? Rand.Range(0, 7) : 0);
					int num3 = Rand.Range(0, 7);
					num2 = 0;
					num3 = 0;
					int num4 = magnitudeManhattan - num2 - num3;
					IntVec2 size2 = ship_Beam.Size;
					num5 = num4 / size2.z;
					if (num5 > 0)
						break;
				}
				IntVec2 a = intVec;
				IntVec2 a2 = toIntVec;
				int num6 = num2;
				IntVec2 size3 = ship_Beam.Size;
				IntVec2 intVec2 = a + a2 * (num6 + size3.z / 2 - 1);
				Thing t = null;
				for (int num7 = 0; num7 < num5; num7++)
				{
					Thing thing2 = ThingMaker.MakeThing(ship_Beam, null);
					thing2.SetFaction(rp.faction, null);
					t = GenSpawn.Spawn(thing2, intVec2.ToIntVec3, map, rp.thingRot.Value, false);
					IntVec2 a3 = intVec2;
					IntVec2 a4 = toIntVec;
					IntVec2 size4 = ship_Beam.Size;
					intVec2 = a3 + a4 * size4.z;
				}
				bool? allowPlacementOffEdge3 = rp.allowPlacementOffEdge;
				if (!allowPlacementOffEdge3.HasValue || allowPlacementOffEdge3.Value)
				{
					BaseGen.symbolStack.Push("ship_populate", rp);
				}
				CellRect rect;
				Rot4 value;
				CellRect rect2;
				Rot4 value2;
				if (rp.thingRot.Value.IsHorizontal)
				{
					rect = rp.rect;
					CellRect cellRect = t.OccupiedRect();
					rect.minZ = cellRect.maxZ + 1;
					value = Rot4.North;
					rect2 = rp.rect;
					CellRect cellRect2 = t.OccupiedRect();
					rect2.maxZ = cellRect2.minZ - 1;
					value2 = Rot4.South;
				}
				else
				{
					rect = rp.rect;
					CellRect cellRect3 = t.OccupiedRect();
					rect.maxX = cellRect3.minX - 1;
					value = Rot4.West;
					rect2 = rp.rect;
					CellRect cellRect4 = t.OccupiedRect();
					rect2.minX = cellRect4.maxX + 1;
					value2 = Rot4.East;
				}
				bool? allowPlacementOffEdge4 = rp.allowPlacementOffEdge;
				if (!allowPlacementOffEdge4.HasValue || allowPlacementOffEdge4.Value || Rand.Value < 0.30000001192092896)
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = rect;
					resolveParams.thingRot = new Rot4?(value);
					resolveParams.allowPlacementOffEdge = new bool?(false);
					BaseGen.symbolStack.Push("ship_spine", resolveParams);
				}
				bool? allowPlacementOffEdge5 = rp.allowPlacementOffEdge;
				if (!allowPlacementOffEdge5.HasValue || allowPlacementOffEdge5.Value || Rand.Value < 0.30000001192092896)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.rect = rect2;
					resolveParams2.thingRot = new Rot4?(value2);
					resolveParams2.allowPlacementOffEdge = new bool?(false);
					BaseGen.symbolStack.Push("ship_spine", resolveParams2);
				}
				ResolveParams resolveParams3 = rp;
				resolveParams3.floorDef = TerrainDefOf.Concrete;
				BaseGen.symbolStack.Push("floor", resolveParams3);
			}
		}
	}
}
