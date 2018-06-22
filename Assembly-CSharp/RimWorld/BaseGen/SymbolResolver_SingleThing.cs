using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B8 RID: 952
	public class SymbolResolver_SingleThing : SymbolResolver
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x0008BAD8 File Offset: 0x00089ED8
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				result = true;
			}
			else
			{
				IntVec3 intVec;
				if (rp.singleThingToSpawn is Pawn)
				{
					ResolveParams rp2 = rp;
					rp2.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
					if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp2, out intVec))
					{
						return false;
					}
				}
				result = (((rp.singleThingDef == null || rp.singleThingDef.category != ThingCategory.Item) && (rp.singleThingToSpawn == null || rp.singleThingToSpawn.def.category != ThingCategory.Item)) || this.TryFindSpawnCellForItem(rp.rect, out intVec));
			}
			return result;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0008BBC0 File Offset: 0x00089FC0
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
			else if (rp.singleThingToSpawn == null || !rp.singleThingToSpawn.Spawned)
			{
				ThingDef thingDef2;
				if (rp.singleThingToSpawn == null)
				{
					ThingDef thingDef;
					if ((thingDef = rp.singleThingDef) == null)
					{
						thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
						where x.IsWeapon || x.IsMedicine || x.IsDrug
						select x).RandomElement<ThingDef>();
					}
					thingDef2 = thingDef;
				}
				else
				{
					thingDef2 = rp.singleThingToSpawn.def;
				}
				Rot4? rot = rp.thingRot;
				IntVec3 loc;
				if (thingDef2.category == ThingCategory.Item)
				{
					rot = new Rot4?(Rot4.North);
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
					bool flag;
					bool flag2;
					loc = this.FindBestSpawnCellForNonItem(rp.rect, thingDef2, ref rot, out flag, out flag2);
					if ((flag || flag2) && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value)
					{
						return;
					}
				}
				if (rot == null)
				{
					Log.Error("Could not resolve rotation. Bug.", false);
				}
				Thing thing;
				if (rp.singleThingToSpawn == null)
				{
					ThingDef stuff;
					if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef2))
					{
						stuff = rp.singleThingStuff;
					}
					else
					{
						stuff = GenStuff.RandomStuffInexpensiveFor(thingDef2, rp.faction);
					}
					thing = ThingMaker.MakeThing(thingDef2, stuff);
					Thing thing2 = thing;
					int? singleThingStackCount = rp.singleThingStackCount;
					thing2.stackCount = ((singleThingStackCount == null) ? 1 : singleThingStackCount.Value);
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
						compQuality.SetQuality(QualityUtility.GenerateQualityBaseGen(), ArtGenerationContext.Outsider);
					}
					if (rp.postThingGenerate != null)
					{
						rp.postThingGenerate(thing);
					}
				}
				else
				{
					thing = rp.singleThingToSpawn;
				}
				thing = GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, rot.Value, WipeMode.Vanish, false);
				if (thing != null && thing.def.category == ThingCategory.Item)
				{
					thing.SetForbidden(true, false);
				}
				if (rp.postThingSpawn != null)
				{
					rp.postThingSpawn(thing);
				}
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0008BEAC File Offset: 0x0008A2AC
		private bool TryFindSpawnCellForItem(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
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
							return false;
						}
					}
					result2 = true;
				}
				return result2;
			}, out result);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0008BEEC File Offset: 0x0008A2EC
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, ref Rot4? rot, out bool hasToWipeBuilding, out bool doesntFit)
		{
			if (!thingDef.rotatable)
			{
				rot = new Rot4?(Rot4.North);
			}
			IntVec3 result3;
			if (rot == null)
			{
				SymbolResolver_SingleThing.tmpRotations.Shuffle<Rot4>();
				for (int i = 0; i < SymbolResolver_SingleThing.tmpRotations.Length; i++)
				{
					IntVec3 result = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[i], out hasToWipeBuilding, out doesntFit);
					if (!hasToWipeBuilding && !doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[i]);
						return result;
					}
				}
				for (int j = 0; j < SymbolResolver_SingleThing.tmpRotations.Length; j++)
				{
					IntVec3 result2 = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[j], out hasToWipeBuilding, out doesntFit);
					if (!doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[j]);
						return result2;
					}
				}
				rot = new Rot4?(Rot4.Random);
				result3 = this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit);
			}
			else
			{
				result3 = this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit);
			}
			return result3;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0008C040 File Offset: 0x0008A440
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, Rot4 rot, out bool hasToWipeBuilding, out bool doesntFit)
		{
			Map map = BaseGen.globalSettings.map;
			if (thingDef.category == ThingCategory.Building)
			{
				foreach (IntVec3 intVec in rect.Cells.InRandomOrder(null))
				{
					CellRect rect2 = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
					if (rect2.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect2, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect2) && GenConstruct.TerrainCanSupport(rect2, map, thingDef))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec;
					}
				}
				foreach (IntVec3 intVec2 in rect.Cells.InRandomOrder(null))
				{
					CellRect rect3 = GenAdj.OccupiedRect(intVec2, rot, thingDef.size);
					if (rect3.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect3, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect3))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec2;
					}
				}
			}
			foreach (IntVec3 intVec3 in rect.Cells.InRandomOrder(null))
			{
				CellRect rect4 = GenAdj.OccupiedRect(intVec3, rot, thingDef.size);
				if (rect4.FullyContainedWithin(rect) && !this.AnyNonStandableCellOrAnyBuildingInside(rect4))
				{
					hasToWipeBuilding = false;
					doesntFit = false;
					return intVec3;
				}
			}
			foreach (IntVec3 intVec4 in rect.Cells.InRandomOrder(null))
			{
				if (GenAdj.OccupiedRect(intVec4, rot, thingDef.size).FullyContainedWithin(rect))
				{
					hasToWipeBuilding = true;
					doesntFit = false;
					return intVec4;
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
			if (cellRect.maxX >= map.Size.x)
			{
				centerCell.x -= cellRect.maxX - map.Size.x + 1;
			}
			if (cellRect.maxZ >= map.Size.z)
			{
				centerCell.z -= cellRect.maxZ - map.Size.z + 1;
			}
			hasToWipeBuilding = true;
			doesntFit = true;
			return centerCell;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0008C3B4 File Offset: 0x0008A7B4
		private bool AnyNonStandableCellOrAnyBuildingInside(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				bool result;
				if (!iterator.Current.Standable(map))
				{
					result = true;
				}
				else
				{
					if (iterator.Current.GetEdifice(map) == null)
					{
						iterator.MoveNext();
						continue;
					}
					result = true;
				}
				return result;
			}
			return false;
		}

		// Token: 0x04000A25 RID: 2597
		private static Rot4[] tmpRotations = new Rot4[]
		{
			Rot4.North,
			Rot4.South,
			Rot4.West,
			Rot4.East
		};
	}
}
