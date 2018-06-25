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
		public PlaceWorker_WatermillGenerator()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 c in CompPowerPlantWater.GroundCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.Medium))
				{
					return new AcceptanceReport("TerrainCannotSupport".Translate());
				}
			}
			foreach (IntVec3 c2 in CompPowerPlantWater.WaterCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c2).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return new AcceptanceReport("MustBeOnMovingWater".Translate());
				}
			}
			return true;
		}

		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList<IntVec3>(), new Color(0.7f, 0.65f, 0.6f));
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList<IntVec3>(), new Color(0.6f, 0.6f, 0.7f));
		}

		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Medium;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
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
					this.$current = TerrainAffordanceDefOf.Medium;
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
