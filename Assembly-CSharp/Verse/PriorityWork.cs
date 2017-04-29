using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class PriorityWork : IExposable
	{
		private const int Timeout = 30000;

		private Pawn pawn;

		private IntVec3 prioritizedCell = IntVec3.Invalid;

		private WorkTypeDef prioritizedWorkType;

		private int prioritizeTick = Find.TickManager.TicksGame;

		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		public WorkTypeDef WorkType
		{
			get
			{
				return this.prioritizedWorkType;
			}
		}

		public PriorityWork()
		{
		}

		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkTypeDef>(ref this.prioritizedWorkType, "prioritizedWorkType");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		public void Set(IntVec3 prioritizedCell, WorkTypeDef prioritizedWorkType)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkType = prioritizedWorkType;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkType = null;
			this.prioritizeTick = 0;
		}

		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.jobQueue.Clear();
			this.pawn.ClearReservations(false);
		}

		public void DrawExtraSelectionOverlays()
		{
			if (this.IsPrioritized)
			{
				GenDraw.DrawLineBetween(this.pawn.DrawPos, this.Cell.ToVector3Shifted());
			}
		}

		[DebuggerHidden]
		public IEnumerable<Gizmo> GetGizmos()
		{
			PriorityWork.<GetGizmos>c__Iterator1B9 <GetGizmos>c__Iterator1B = new PriorityWork.<GetGizmos>c__Iterator1B9();
			<GetGizmos>c__Iterator1B.<>f__this = this;
			PriorityWork.<GetGizmos>c__Iterator1B9 expr_0E = <GetGizmos>c__Iterator1B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
