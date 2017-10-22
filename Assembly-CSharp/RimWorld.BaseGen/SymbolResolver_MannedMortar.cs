using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_MannedMortar : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (!base.CanResolve(rp))
			{
				return false;
			}
			int num = 0;
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				if (iterator.Current.Standable(map))
				{
					num++;
				}
				iterator.MoveNext();
			}
			if (num < 2)
			{
				return false;
			}
			return true;
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true);
			Rot4? thingRot = rp.thingRot;
			Rot4 rot = (!thingRot.HasValue) ? Rot4.Random : thingRot.Value;
			ThingDef thingDef = rp.mortarDef ?? (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Building && x.building.IsMortar && x != ThingDefOf.Turret_MortarEMP
			select x).RandomElement();
			IntVec3 intVec = default(IntVec3);
			if (this.TryFindMortarSpawnCell(rp.rect, rot, thingDef, out intVec))
			{
				if (thingDef.HasComp(typeof(CompMannable)))
				{
					IntVec3 c = Thing.InteractionCellWhenAt(thingDef, intVec, rot, map);
					Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_ManTurrets(), map, null);
					int tile = map.Tile;
					PawnGenerationRequest value = new PawnGenerationRequest(faction.RandomPawnKind(), faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, true, true, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
					ResolveParams resolveParams = rp;
					resolveParams.faction = faction;
					resolveParams.singlePawnGenerationRequest = new PawnGenerationRequest?(value);
					resolveParams.rect = CellRect.SingleCell(c);
					resolveParams.singlePawnLord = singlePawnLord;
					BaseGen.symbolStack.Push("pawn", resolveParams);
				}
				if (thingDef.building.turretShellDef != null)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.faction = faction;
					resolveParams2.singleThingDef = thingDef.building.turretShellDef;
					resolveParams2.singleThingStackCount = new int?(Rand.RangeInclusive(5, Mathf.Min(8, thingDef.building.turretShellDef.stackLimit)));
					BaseGen.symbolStack.Push("thing", resolveParams2);
				}
				ResolveParams resolveParams3 = rp;
				resolveParams3.faction = faction;
				resolveParams3.singleThingDef = thingDef;
				resolveParams3.rect = CellRect.SingleCell(intVec);
				resolveParams3.thingRot = new Rot4?(rot);
				BaseGen.symbolStack.Push("thing", resolveParams3);
			}
		}

		private bool TryFindMortarSpawnCell(CellRect rect, Rot4 rot, ThingDef mortarDef, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			Predicate<CellRect> edgeTouchCheck;
			if (rot == Rot4.North)
			{
				CellRect rect5;
				edgeTouchCheck = (Predicate<CellRect>)((CellRect x) => x.Cells.Any((Func<IntVec3, bool>)((IntVec3 y) => y.z == rect5.maxZ)));
			}
			else if (rot == Rot4.South)
			{
				CellRect rect4;
				edgeTouchCheck = (Predicate<CellRect>)((CellRect x) => x.Cells.Any((Func<IntVec3, bool>)((IntVec3 y) => y.z == rect4.minZ)));
			}
			else if (rot == Rot4.West)
			{
				CellRect rect3;
				edgeTouchCheck = (Predicate<CellRect>)((CellRect x) => x.Cells.Any((Func<IntVec3, bool>)((IntVec3 y) => y.x == rect3.minX)));
			}
			else
			{
				CellRect rect2;
				edgeTouchCheck = (Predicate<CellRect>)((CellRect x) => x.Cells.Any((Func<IntVec3, bool>)((IntVec3 y) => y.x == rect2.maxX)));
			}
			return CellFinder.TryFindRandomCellInsideWith(rect, (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				CellRect obj = GenAdj.OccupiedRect(x, rot, mortarDef.size);
				if (!Thing.InteractionCellWhenAt(mortarDef, x, rot, map).Standable(map))
				{
					return false;
				}
				return obj.FullyContainedWithin(rect) && edgeTouchCheck(obj);
			}, out cell);
		}
	}
}
