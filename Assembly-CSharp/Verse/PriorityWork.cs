using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000AA3 RID: 2723
	public class PriorityWork : IExposable
	{
		// Token: 0x0400266E RID: 9838
		private Pawn pawn;

		// Token: 0x0400266F RID: 9839
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x04002670 RID: 9840
		private WorkTypeDef prioritizedWorkType = null;

		// Token: 0x04002671 RID: 9841
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x04002672 RID: 9842
		private const int Timeout = 30000;

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00202673 File Offset: 0x00200A73
		public PriorityWork()
		{
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0020269E File Offset: 0x00200A9E
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x002026D0 File Offset: 0x00200AD0
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

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x00202720 File Offset: 0x00200B20
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003CBB RID: 15547 RVA: 0x0020273C File Offset: 0x00200B3C
		public WorkTypeDef WorkType
		{
			get
			{
				return this.prioritizedWorkType;
			}
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00202758 File Offset: 0x00200B58
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkTypeDef>(ref this.prioritizedWorkType, "prioritizedWorkType");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x002027A2 File Offset: 0x00200BA2
		public void Set(IntVec3 prioritizedCell, WorkTypeDef prioritizedWorkType)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkType = prioritizedWorkType;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x002027C3 File Offset: 0x00200BC3
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkType = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x002027DF File Offset: 0x00200BDF
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs();
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x002027F8 File Offset: 0x00200BF8
		public IEnumerable<Gizmo> GetGizmos()
		{
			if ((this.IsPrioritized || (this.pawn.CurJob != null && this.pawn.CurJob.playerForced) || this.pawn.jobs.jobQueue.AnyPlayerForced) && !this.pawn.Drafted)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						this.ClearPrioritizedWorkAndJobQueue();
						if (this.pawn.CurJob.playerForced)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKey = 6165612
				};
			}
			yield break;
		}
	}
}
