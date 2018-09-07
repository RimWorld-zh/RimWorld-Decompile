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
	public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
	{
		public WorkGiver_ConstructSmoothWall()
		{
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall))
			{
				yield return des.target.Cell;
			}
			yield break;
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (c.IsForbidden(pawn) || pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176, false);
				pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
				return false;
			}
			LocalTargetInfo target = edifice;
			if (pawn.CanReserve(target, 1, -1, null, forced))
			{
				target = c;
				if (pawn.CanReserve(target, 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkCellsGlobal>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Pawn pawn;

			internal IEnumerator<Designation> $locvar0;

			internal Designation <des>__1;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (pawn.Faction != Faction.OfPlayer)
					{
						return false;
					}
					enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall).GetEnumerator();
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
						this.$current = des.target.Cell;
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorkGiver_ConstructSmoothWall.<PotentialWorkCellsGlobal>c__Iterator0 <PotentialWorkCellsGlobal>c__Iterator = new WorkGiver_ConstructSmoothWall.<PotentialWorkCellsGlobal>c__Iterator0();
				<PotentialWorkCellsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkCellsGlobal>c__Iterator;
			}
		}
	}
}
