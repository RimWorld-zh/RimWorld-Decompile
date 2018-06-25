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
	public class WorkGiver_RearmTraps : WorkGiver_Scanner
	{
		public WorkGiver_RearmTraps()
		{
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RearmTrap))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RearmTrap) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = t.Position.GetThingList(t.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] != t && thingList[i].def.category == ThingCategory.Item)
						{
							IntVec3 intVec;
							if (thingList[i].IsForbidden(pawn) || thingList[i].IsInValidStorage() || !HaulAIUtility.CanHaulAside(pawn, thingList[i], out intVec))
							{
								return false;
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			List<Thing> thingList = t.Position.GetThingList(t.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] != t && thingList[i].def.category == ThingCategory.Item)
				{
					Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[i]);
					if (job != null)
					{
						return job;
					}
				}
			}
			return new Job(JobDefOf.RearmTrap, t);
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkThingsGlobal>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn pawn;

			internal IEnumerator<Designation> $locvar0;

			internal Designation <des>__1;

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
					enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RearmTrap).GetEnumerator();
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
					if (enumerator.MoveNext())
					{
						des = enumerator.Current;
						this.$current = des.target.Thing;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
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
				WorkGiver_RearmTraps.<PotentialWorkThingsGlobal>c__Iterator0 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_RearmTraps.<PotentialWorkThingsGlobal>c__Iterator0();
				<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkThingsGlobal>c__Iterator;
			}
		}
	}
}
