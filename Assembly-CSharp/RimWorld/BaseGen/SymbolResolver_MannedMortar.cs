using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CC RID: 972
	public class SymbolResolver_MannedMortar : SymbolResolver
	{
		// Token: 0x04000A38 RID: 2616
		private const float MaxShellDefMarketValue = 250f;

		// Token: 0x060010C2 RID: 4290 RVA: 0x0008E9D8 File Offset: 0x0008CDD8
		public override bool CanResolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else
			{
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
				result = (num >= 2);
			}
			return result;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0008EA5C File Offset: 0x0008CE5C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction;
			if ((faction = rp.faction) == null)
			{
				faction = (Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Industrial) ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined));
			}
			Faction faction2 = faction;
			Rot4? thingRot = rp.thingRot;
			Rot4 rot = (thingRot == null) ? Rot4.Random : thingRot.Value;
			ThingDef thingDef;
			if ((thingDef = rp.mortarDef) == null)
			{
				thingDef = (from x in DefDatabase<ThingDef>.AllDefsListForReading
				where x.category == ThingCategory.Building && x.building.IsMortar
				select x).RandomElement<ThingDef>();
			}
			ThingDef thingDef2 = thingDef;
			IntVec3 intVec;
			if (this.TryFindMortarSpawnCell(rp.rect, rot, thingDef2, out intVec))
			{
				if (thingDef2.HasComp(typeof(CompMannable)))
				{
					IntVec3 c = ThingUtility.InteractionCellWhenAt(thingDef2, intVec, rot, map);
					Lord singlePawnLord = LordMaker.MakeNewLord(faction2, new LordJob_ManTurrets(), map, null);
					PawnKindDef kind = faction2.RandomPawnKind();
					Faction faction3 = faction2;
					int tile = map.Tile;
					PawnGenerationRequest value = new PawnGenerationRequest(kind, faction3, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, true, true, false, false, false, null, null, null, null, null, null, null, null);
					ResolveParams resolveParams = rp;
					resolveParams.faction = faction2;
					resolveParams.singlePawnGenerationRequest = new PawnGenerationRequest?(value);
					resolveParams.rect = CellRect.SingleCell(c);
					resolveParams.singlePawnLord = singlePawnLord;
					BaseGen.symbolStack.Push("pawn", resolveParams);
				}
				ThingDef turret = thingDef2;
				bool allowEMP = false;
				TechLevel techLevel = faction2.def.techLevel;
				ThingDef thingDef3 = TurretGunUtility.TryFindRandomShellDef(turret, allowEMP, true, techLevel, false, 250f);
				if (thingDef3 != null)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.faction = faction2;
					resolveParams2.singleThingDef = thingDef3;
					resolveParams2.singleThingStackCount = new int?(Rand.RangeInclusive(5, Mathf.Min(8, thingDef3.stackLimit)));
					BaseGen.symbolStack.Push("thing", resolveParams2);
				}
				ResolveParams resolveParams3 = rp;
				resolveParams3.faction = faction2;
				resolveParams3.singleThingDef = thingDef2;
				resolveParams3.rect = CellRect.SingleCell(intVec);
				resolveParams3.thingRot = new Rot4?(rot);
				BaseGen.symbolStack.Push("thing", resolveParams3);
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0008ECBC File Offset: 0x0008D0BC
		private bool TryFindMortarSpawnCell(CellRect rect, Rot4 rot, ThingDef mortarDef, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			Predicate<CellRect> edgeTouchCheck;
			if (rot == Rot4.North)
			{
				edgeTouchCheck = ((CellRect x) => x.Cells.Any((IntVec3 y) => y.z == rect.maxZ));
			}
			else if (rot == Rot4.South)
			{
				edgeTouchCheck = ((CellRect x) => x.Cells.Any((IntVec3 y) => y.z == rect.minZ));
			}
			else if (rot == Rot4.West)
			{
				edgeTouchCheck = ((CellRect x) => x.Cells.Any((IntVec3 y) => y.x == rect.minX));
			}
			else
			{
				edgeTouchCheck = ((CellRect x) => x.Cells.Any((IntVec3 y) => y.x == rect.maxX));
			}
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 x)
			{
				CellRect obj = GenAdj.OccupiedRect(x, rot, mortarDef.size);
				return ThingUtility.InteractionCellWhenAt(mortarDef, x, rot, map).Standable(map) && obj.FullyContainedWithin(rect) && edgeTouchCheck(obj);
			}, out cell);
		}
	}
}
