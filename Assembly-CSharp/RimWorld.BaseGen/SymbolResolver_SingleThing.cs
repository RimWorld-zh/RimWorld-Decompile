using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_SingleThing : SymbolResolver
	{
		private static Rot4[] tmpRotations = new Rot4[4]
		{
			Rot4.North,
			Rot4.South,
			Rot4.West,
			Rot4.East
		};

		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
				goto IL_00d8;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				result = true;
				goto IL_00d8;
			}
			IntVec3 intVec = default(IntVec3);
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams rp2 = rp;
				rp2.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp2, out intVec))
				{
					result = false;
					goto IL_00d8;
				}
			}
			if (rp.singleThingDef != null && rp.singleThingDef.category == ThingCategory.Item)
			{
				goto IL_00b5;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.def.category == ThingCategory.Item)
				goto IL_00b5;
			goto IL_00d1;
			IL_00b5:
			if (!this.TryFindSpawnCellForItem(rp.rect, out intVec))
			{
				result = false;
				goto IL_00d8;
			}
			goto IL_00d1;
			IL_00d8:
			return result;
			IL_00d1:
			result = true;
			goto IL_00d8;
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
				Rot4? nullable = rp.thingRot;
				IntVec3 loc = default(IntVec3);
				if (thingDef.category == ThingCategory.Item)
				{
					nullable = new Rot4?(Rot4.North);
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
					bool flag = default(bool);
					bool flag2 = default(bool);
					loc = this.FindBestSpawnCellForNonItem(rp.rect, thingDef, ref nullable, out flag, out flag2);
					if ((flag || flag2) && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.HasValue && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value)
						return;
				}
				if (!nullable.HasValue)
				{
					Log.Error("Could not resolve rotation. Bug.");
				}
				Thing thing;
				if (rp.singleThingToSpawn == null)
				{
					ThingDef stuff = (rp.singleThingStuff == null || !rp.singleThingStuff.stuffProps.CanMake(thingDef)) ? GenStuff.RandomStuffByCommonalityFor(thingDef, (rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined) : rp.singleThingStuff;
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
					if ((object)rp.postThingGenerate != null)
					{
						rp.postThingGenerate(thing);
					}
				}
				else
				{
					thing = rp.singleThingToSpawn;
				}
				thing = GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, nullable.Value, false);
				if (thing != null && thing.def.category == ThingCategory.Item)
				{
					thing.SetForbidden(true, false);
				}
				if ((object)rp.postThingSpawn != null)
				{
					rp.postThingSpawn(thing);
				}
			}
		}

		private bool TryFindSpawnCellForItem(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				bool result2;
				if (c.GetFirstItem(map) != null)
				{
					result2 = false;
				}
				else
				{
					if (!c.Standable(map))
					{
						SurfaceType surfaceType = c.GetSurfaceType(map);
						if (surfaceType != SurfaceType.Item && surfaceType != SurfaceType.Eat)
						{
							result2 = false;
							goto IL_0055;
						}
					}
					result2 = true;
				}
				goto IL_0055;
				IL_0055:
				return result2;
			}, out result);
		}

		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, ref Rot4? rot, out bool hasToWipeBuilding, out bool doesntFit)
		{
			if (!thingDef.rotatable)
			{
				rot = new Rot4?(Rot4.North);
			}
			int i;
			IntVec3 intVec;
			int j;
			IntVec3 intVec2;
			IntVec3 result;
			if (!rot.HasValue)
			{
				SymbolResolver_SingleThing.tmpRotations.Shuffle();
				for (i = 0; i < SymbolResolver_SingleThing.tmpRotations.Length; i++)
				{
					intVec = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[i], out hasToWipeBuilding, out doesntFit);
					if (!hasToWipeBuilding && !doesntFit)
						goto IL_006a;
				}
				for (j = 0; j < SymbolResolver_SingleThing.tmpRotations.Length; j++)
				{
					intVec2 = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[j], out hasToWipeBuilding, out doesntFit);
					if (!doesntFit)
						goto IL_00cd;
				}
				rot = new Rot4?(Rot4.Random);
				result = this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit);
			}
			else
			{
				result = this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit);
			}
			goto IL_0143;
			IL_00cd:
			rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[j]);
			result = intVec2;
			goto IL_0143;
			IL_0143:
			return result;
			IL_006a:
			rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[i]);
			result = intVec;
			goto IL_0143;
		}

		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, Rot4 rot, out bool hasToWipeBuilding, out bool doesntFit)
		{
			Map map = BaseGen.globalSettings.map;
			if (thingDef.category == ThingCategory.Building)
			{
				foreach (IntVec3 item in rect.Cells.InRandomOrder(null))
				{
					CellRect rect2 = GenAdj.OccupiedRect(item, rot, thingDef.size);
					if (rect2.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect2, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect2) && GenConstruct.TerrainCanSupport(rect2, map, thingDef))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return item;
					}
				}
				foreach (IntVec3 item2 in rect.Cells.InRandomOrder(null))
				{
					CellRect rect3 = GenAdj.OccupiedRect(item2, rot, thingDef.size);
					if (rect3.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect3, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect3))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return item2;
					}
				}
			}
			foreach (IntVec3 item3 in rect.Cells.InRandomOrder(null))
			{
				CellRect rect4 = GenAdj.OccupiedRect(item3, rot, thingDef.size);
				if (rect4.FullyContainedWithin(rect) && !this.AnyNonStandableCellOrAnyBuildingInside(rect4))
				{
					hasToWipeBuilding = false;
					doesntFit = false;
					return item3;
				}
			}
			foreach (IntVec3 item4 in rect.Cells.InRandomOrder(null))
			{
				if (GenAdj.OccupiedRect(item4, rot, thingDef.size).FullyContainedWithin(rect))
				{
					hasToWipeBuilding = true;
					doesntFit = false;
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
			hasToWipeBuilding = true;
			doesntFit = true;
			return centerCell;
		}

		private bool AnyNonStandableCellOrAnyBuildingInside(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect.CellRectIterator iterator = rect.GetIterator();
			bool result;
			while (true)
			{
				if (!iterator.Done())
				{
					if (!iterator.Current.Standable(map))
					{
						result = true;
						break;
					}
					if (iterator.Current.GetEdifice(map) != null)
					{
						result = true;
						break;
					}
					iterator.MoveNext();
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
