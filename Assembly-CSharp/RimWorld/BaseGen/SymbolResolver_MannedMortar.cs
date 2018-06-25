using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_MannedMortar : SymbolResolver
	{
		private const float MaxShellDefMarketValue = 250f;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		public SymbolResolver_MannedMortar()
		{
		}

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

		[CompilerGenerated]
		private static bool <Resolve>m__0(ThingDef x)
		{
			return x.category == ThingCategory.Building && x.building.IsMortar;
		}

		[CompilerGenerated]
		private sealed class <TryFindMortarSpawnCell>c__AnonStorey0
		{
			internal CellRect rect;

			internal Rot4 rot;

			internal ThingDef mortarDef;

			internal Map map;

			internal Predicate<CellRect> edgeTouchCheck;

			public <TryFindMortarSpawnCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(CellRect x)
			{
				return x.Cells.Any((IntVec3 y) => y.z == this.rect.maxZ);
			}

			internal bool <>m__1(CellRect x)
			{
				return x.Cells.Any((IntVec3 y) => y.z == this.rect.minZ);
			}

			internal bool <>m__2(CellRect x)
			{
				return x.Cells.Any((IntVec3 y) => y.x == this.rect.minX);
			}

			internal bool <>m__3(CellRect x)
			{
				return x.Cells.Any((IntVec3 y) => y.x == this.rect.maxX);
			}

			internal bool <>m__4(IntVec3 x)
			{
				CellRect obj = GenAdj.OccupiedRect(x, this.rot, this.mortarDef.size);
				return ThingUtility.InteractionCellWhenAt(this.mortarDef, x, this.rot, this.map).Standable(this.map) && obj.FullyContainedWithin(this.rect) && this.edgeTouchCheck(obj);
			}

			internal bool <>m__5(IntVec3 y)
			{
				return y.z == this.rect.maxZ;
			}

			internal bool <>m__6(IntVec3 y)
			{
				return y.z == this.rect.minZ;
			}

			internal bool <>m__7(IntVec3 y)
			{
				return y.x == this.rect.minX;
			}

			internal bool <>m__8(IntVec3 y)
			{
				return y.x == this.rect.maxX;
			}
		}
	}
}
