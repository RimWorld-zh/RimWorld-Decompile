using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_SingleThing : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				return true;
			}
			IntVec3 intVec = default(IntVec3);
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams rp2 = rp;
				rp2.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp2, out intVec))
				{
					return false;
				}
			}
			if (rp.singleThingDef != null && rp.singleThingDef.category == ThingCategory.Item)
			{
				goto IL_00a3;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.def.category == ThingCategory.Item)
				goto IL_00a3;
			goto IL_00b9;
			IL_00b9:
			return true;
			IL_00a3:
			if (!this.TryFindSpawnCellForItem(rp.rect, out intVec))
			{
				return false;
			}
			goto IL_00b9;
		}

		public override void Resolve(ResolveParams rp)
		{
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
			else
			{
				if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
					return;
				ThingDef thingDef = (rp.singleThingToSpawn != null) ? rp.singleThingToSpawn.def : (rp.singleThingDef ?? (from x in ItemCollectionGeneratorUtility.allGeneratableItems
				where x.IsWeapon || x.IsMedicine || x.IsDrug
				select x).RandomElement());
				Rot4? thingRot = rp.thingRot;
				Rot4 rot = (!thingRot.HasValue) ? Rot4.North : thingRot.Value;
				if (!thingDef.rotatable)
				{
					rot = Rot4.North;
				}
				IntVec3 loc = default(IntVec3);
				if (thingDef.category == ThingCategory.Item)
				{
					if (!this.TryFindSpawnCellForItem(rp.rect, out loc))
					{
						if (rp.singleThingToSpawn != null)
						{
							rp.singleThingToSpawn.Destroy(DestroyMode.Vanish);
						}
						return;
					}
				}
				else
				{
					loc = this.FindBestSpawnCellForNonItem(rp.rect, thingDef, rot);
				}
				Thing thing;
				if (rp.singleThingToSpawn == null)
				{
					ThingDef stuff = rp.singleThingStuff ?? GenStuff.DefaultStuffFor(thingDef);
					thing = ThingMaker.MakeThing(thingDef, stuff);
					Thing obj = thing;
					int? singleThingStackCount = rp.singleThingStackCount;
					obj.stackCount = ((!singleThingStackCount.HasValue) ? 1 : singleThingStackCount.Value);
					if (thing.stackCount <= 0)
					{
						thing.stackCount = 1;
					}
					if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
					{
						thing.SetFaction(rp.faction, null);
					}
					CompQuality compQuality = thing.TryGetComp<CompQuality>();
					if (compQuality != null)
					{
						compQuality.SetQuality(QualityUtility.RandomBaseGenItemQuality(), ArtGenerationContext.Outsider);
					}
				}
				else
				{
					thing = rp.singleThingToSpawn;
				}
				thing = GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, rot, false);
				if (thing != null && thing.def.category == ThingCategory.Item)
				{
					thing.SetForbidden(true, false);
				}
			}
		}

		private bool TryFindSpawnCellForItem(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if (c.GetFirstItem(map) != null)
				{
					return false;
				}
				if (!c.Standable(map))
				{
					SurfaceType surfaceType = c.GetSurfaceType(map);
					if (surfaceType != SurfaceType.Item && surfaceType != SurfaceType.Eat)
					{
						return false;
					}
				}
				return true;
			}, out result);
		}

		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, Rot4 rot)
		{
			Map map = BaseGen.globalSettings.map;
			if (thingDef.category == ThingCategory.Building)
			{
				foreach (IntVec3 item in rect.Cells.InRandomOrder(null))
				{
					CellRect rect2 = GenAdj.OccupiedRect(item, rot, thingDef.size);
					if (rect2.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorCardinalAdjacentTo(rect2, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect2) && GenConstruct.TerrainCanSupport(rect2, map, thingDef))
					{
						return item;
					}
				}
				foreach (IntVec3 item2 in rect.Cells.InRandomOrder(null))
				{
					CellRect rect3 = GenAdj.OccupiedRect(item2, rot, thingDef.size);
					if (rect3.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorCardinalAdjacentTo(rect3, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect3))
					{
						return item2;
					}
				}
			}
			foreach (IntVec3 item3 in rect.Cells.InRandomOrder(null))
			{
				CellRect rect4 = GenAdj.OccupiedRect(item3, rot, thingDef.size);
				if (rect4.FullyContainedWithin(rect) && !this.AnyNonStandableCellOrAnyBuildingInside(rect4))
				{
					return item3;
				}
			}
			foreach (IntVec3 item4 in rect.Cells.InRandomOrder(null))
			{
				if (GenAdj.OccupiedRect(item4, rot, thingDef.size).FullyContainedWithin(rect))
				{
					return item4;
				}
			}
			IntVec3 centerCell = rect.CenterCell;
			CellRect cellRect = GenAdj.OccupiedRect(centerCell, rot, thingDef.size);
			if (cellRect.minX < 0)
			{
				centerCell.x += -cellRect.minX;
			}
			if (cellRect.minZ < 0)
			{
				centerCell.z += -cellRect.minZ;
			}
			int maxX = cellRect.maxX;
			IntVec3 size = map.Size;
			if (maxX >= size.x)
			{
				int x = centerCell.x;
				int maxX2 = cellRect.maxX;
				IntVec3 size2 = map.Size;
				centerCell.x = x - (maxX2 - size2.x + 1);
			}
			int maxZ = cellRect.maxZ;
			IntVec3 size3 = map.Size;
			if (maxZ >= size3.z)
			{
				int z = centerCell.z;
				int maxZ2 = cellRect.maxZ;
				IntVec3 size4 = map.Size;
				centerCell.z = z - (maxZ2 - size4.z + 1);
			}
			return centerCell;
		}

		private bool AnyNonStandableCellOrAnyBuildingInside(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				if (!iterator.Current.Standable(map))
				{
					return true;
				}
				if (iterator.Current.GetEdifice(map) != null)
				{
					return true;
				}
				iterator.MoveNext();
			}
			return false;
		}
	}
}
