using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_WatermillGenerator : PlaceWorker
	{
		private static List<Thing> waterMills = new List<Thing>();

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		public PlaceWorker_WatermillGenerator()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 c in CompPowerPlantWater.GroundCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					return new AcceptanceReport("TerrainCannotSupport".Translate());
				}
			}
			if (!this.WaterCellsPresent(loc, rot, map))
			{
				return new AcceptanceReport("MustBeOnMovingWater".Translate());
			}
			return true;
		}

		private bool WaterCellsPresent(IntVec3 loc, Rot4 rot, Map map)
		{
			foreach (IntVec3 c in CompPowerPlantWater.WaterCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return false;
				}
			}
			return true;
		}

		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList<IntVec3>(), Color.white);
			Color color = (!this.WaterCellsPresent(loc, rot, Find.CurrentMap)) ? Designator_Place.CannotPlaceColor.ToOpaque() : Designator_Place.CanPlaceColor.ToOpaque();
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList<IntVec3>(), color);
			bool flag = false;
			CellRect cellRect = CompPowerPlantWater.WaterUseRect(loc, rot);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator).Cast<Thing>());
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			foreach (Thing thing in PlaceWorker_WatermillGenerator.waterMills)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(thing.Position, thing.Rotation).ToList<IntVec3>(), new Color(0.2f, 0.2f, 1f));
				if (cellRect.Overlaps(CompPowerPlantWater.WaterUseRect(thing.Position, thing.Rotation)))
				{
					flag = true;
				}
			}
			PlaceWorker_WatermillGenerator.waterMills.Clear();
			Color color2 = (!flag) ? Designator_Place.CanPlaceColor.ToOpaque() : new Color(1f, 0.6f, 0f);
			if (!flag || Time.realtimeSinceStartup % 0.4f < 0.2f)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(loc, rot).ToList<IntVec3>(), color2);
			}
		}

		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Heavy;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PlaceWorker_WatermillGenerator()
		{
		}

		[CompilerGenerated]
		private static bool <DrawGhost>m__0(Thing t)
		{
			return t.def.entityDefToBuild == ThingDefOf.WatermillGenerator;
		}

		[CompilerGenerated]
		private static bool <DrawGhost>m__1(Thing t)
		{
			return t.def.entityDefToBuild == ThingDefOf.WatermillGenerator;
		}

		[CompilerGenerated]
		private sealed class <DisplayAffordances>c__Iterator0 : IEnumerable, IEnumerable<TerrainAffordanceDef>, IEnumerator, IDisposable, IEnumerator<TerrainAffordanceDef>
		{
			internal TerrainAffordanceDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <DisplayAffordances>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = TerrainAffordanceDefOf.Heavy;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = TerrainAffordanceDefOf.MovingFluid;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			TerrainAffordanceDef IEnumerator<TerrainAffordanceDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.TerrainAffordanceDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<TerrainAffordanceDef> IEnumerable<TerrainAffordanceDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new PlaceWorker_WatermillGenerator.<DisplayAffordances>c__Iterator0();
			}
		}
	}
}
