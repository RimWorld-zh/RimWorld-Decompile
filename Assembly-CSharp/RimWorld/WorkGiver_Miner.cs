using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		private static string NoPathTrans;

		public WorkGiver_Miner()
		{
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public static void ResetStaticData()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine))
			{
				bool mayBeAccessible = false;
				for (int j = 0; j < 8; j++)
				{
					IntVec3 c = des.target.Cell + GenAdj.AdjacentCells[j];
					if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
					{
						mayBeAccessible = true;
						break;
					}
				}
				if (mayBeAccessible)
				{
					Mineable i = des.target.Cell.GetFirstMineable(pawn.Map);
					if (i != null)
					{
						yield return i;
					}
				}
			}
			yield break;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!t.def.mineable)
			{
				result = null;
			}
			else if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null)
			{
				result = null;
			}
			else if (!pawn.CanReserve(t, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
						if (intVec2.InBounds(t.Map))
						{
							if (ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn))
							{
								if (intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
								{
									Thing thing = null;
									List<Thing> thingList = intVec2.GetThingList(t.Map);
									for (int k = 0; k < thingList.Count; k++)
									{
										if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
										{
											thing = thingList[k];
											break;
										}
									}
									if (thing != null)
									{
										Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
										if (job != null)
										{
											return job;
										}
										JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
										return null;
									}
								}
							}
						}
					}
					JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Mine, t, 1500, true);
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkThingsGlobal>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn pawn;

			internal IEnumerator<Designation> $locvar0;

			internal Designation <des>__1;

			internal bool <mayBeAccessible>__2;

			internal Mineable <m>__2;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PotentialWorkThingsGlobal>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						des = enumerator.Current;
						mayBeAccessible = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 c = des.target.Cell + GenAdj.AdjacentCells[j];
							if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
							{
								mayBeAccessible = true;
								break;
							}
						}
						if (mayBeAccessible)
						{
							i = des.target.Cell.GetFirstMineable(pawn.Map);
							if (i != null)
							{
								this.$current = i;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorkGiver_Miner.<PotentialWorkThingsGlobal>c__Iterator0 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_Miner.<PotentialWorkThingsGlobal>c__Iterator0();
				<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkThingsGlobal>c__Iterator;
			}
		}
	}
}
