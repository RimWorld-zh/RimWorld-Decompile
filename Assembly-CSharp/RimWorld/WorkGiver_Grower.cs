using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		protected static ThingDef wantedPlantDef = null;

		protected WorkGiver_Grower()
		{
		}

		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Profiler.BeginSample("Grow find cell");
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < bList.Count; i++)
			{
				Building_PlantGrower b = bList[i] as Building_PlantGrower;
				if (b != null)
				{
					if (this.ExtraRequirements(b, pawn))
					{
						if (!b.IsForbidden(pawn))
						{
							if (pawn.CanReach(b, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
							{
								if (!b.IsBurning())
								{
									CellRect.CellRectIterator cri = b.OccupiedRect().GetIterator();
									while (!cri.Done())
									{
										yield return cri.Current;
										cri.MoveNext();
									}
									WorkGiver_Grower.wantedPlantDef = null;
								}
							}
						}
					}
				}
			}
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int j = 0; j < zonesList.Count; j++)
			{
				Zone_Growing growZone = zonesList[j] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487, false);
					}
					else if (this.ExtraRequirements(growZone, pawn))
					{
						if (!growZone.ContainsStaticFire)
						{
							if (pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
							{
								for (int k = 0; k < growZone.cells.Count; k++)
								{
									yield return growZone.cells[k];
								}
								WorkGiver_Grower.wantedPlantDef = null;
							}
						}
					}
				}
			}
			WorkGiver_Grower.wantedPlantDef = null;
			Profiler.EndSample();
			yield break;
		}

		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			ThingDef result;
			if (plantToGrowSettable == null)
			{
				result = null;
			}
			else
			{
				result = plantToGrowSettable.GetPlantDefToGrow();
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WorkGiver_Grower()
		{
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkCellsGlobal>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Pawn pawn;

			internal Danger <maxDanger>__0;

			internal List<Building> <bList>__0;

			internal int <i>__1;

			internal Building_PlantGrower <b>__2;

			internal CellRect.CellRectIterator <cri>__3;

			internal List<Zone> <zonesList>__0;

			internal int <i>__4;

			internal Zone_Growing <growZone>__5;

			internal int <j>__6;

			internal WorkGiver_Grower $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PotentialWorkCellsGlobal>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					Profiler.BeginSample("Grow find cell");
					maxDanger = pawn.NormalMaxDanger();
					bList = pawn.Map.listerBuildings.allBuildingsColonist;
					i = 0;
					goto IL_184;
				case 1u:
					cri.MoveNext();
					break;
				case 2u:
					k++;
					goto IL_2E0;
				default:
					return false;
				}
				IL_15F:
				if (!cri.Done())
				{
					this.$current = cri.Current;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				WorkGiver_Grower.wantedPlantDef = null;
				IL_176:
				i++;
				IL_184:
				if (i >= bList.Count)
				{
					WorkGiver_Grower.wantedPlantDef = null;
					zonesList = pawn.Map.zoneManager.AllZones;
					j = 0;
					goto IL_310;
				}
				b = (bList[i] as Building_PlantGrower);
				if (b == null)
				{
					goto IL_176;
				}
				if (!this.ExtraRequirements(b, pawn))
				{
					goto IL_176;
				}
				if (b.IsForbidden(pawn))
				{
					goto IL_176;
				}
				if (!pawn.CanReach(b, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
				{
					goto IL_176;
				}
				if (b.IsBurning())
				{
					goto IL_176;
				}
				cri = b.OccupiedRect().GetIterator();
				goto IL_15F;
				IL_2E0:
				if (k < growZone.cells.Count)
				{
					this.$current = growZone.cells[k];
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				WorkGiver_Grower.wantedPlantDef = null;
				IL_302:
				j++;
				IL_310:
				if (j >= zonesList.Count)
				{
					WorkGiver_Grower.wantedPlantDef = null;
					Profiler.EndSample();
					this.$PC = -1;
				}
				else
				{
					growZone = (zonesList[j] as Zone_Growing);
					if (growZone == null)
					{
						goto IL_302;
					}
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487, false);
						goto IL_302;
					}
					if (!this.ExtraRequirements(growZone, pawn))
					{
						goto IL_302;
					}
					if (growZone.ContainsStaticFire)
					{
						goto IL_302;
					}
					if (!pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
					{
						goto IL_302;
					}
					k = 0;
					goto IL_2E0;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorkGiver_Grower.<PotentialWorkCellsGlobal>c__Iterator0 <PotentialWorkCellsGlobal>c__Iterator = new WorkGiver_Grower.<PotentialWorkCellsGlobal>c__Iterator0();
				<PotentialWorkCellsGlobal>c__Iterator.$this = this;
				<PotentialWorkCellsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkCellsGlobal>c__Iterator;
			}
		}
	}
}
