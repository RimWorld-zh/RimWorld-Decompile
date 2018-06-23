using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000AA1 RID: 2721
	public class PriorityWork : IExposable
	{
		// Token: 0x0400266D RID: 9837
		private Pawn pawn;

		// Token: 0x0400266E RID: 9838
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x0400266F RID: 9839
		private WorkTypeDef prioritizedWorkType = null;

		// Token: 0x04002670 RID: 9840
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x04002671 RID: 9841
		private const int Timeout = 30000;

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00202547 File Offset: 0x00200947
		public PriorityWork()
		{
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x00202572 File Offset: 0x00200972
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x002025A4 File Offset: 0x002009A4
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
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x002025F4 File Offset: 0x002009F4
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00202610 File Offset: 0x00200A10
		public WorkTypeDef WorkType
		{
			get
			{
				return this.prioritizedWorkType;
			}
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0020262C File Offset: 0x00200A2C
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkTypeDef>(ref this.prioritizedWorkType, "prioritizedWorkType");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00202676 File Offset: 0x00200A76
		public void Set(IntVec3 prioritizedCell, WorkTypeDef prioritizedWorkType)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkType = prioritizedWorkType;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00202697 File Offset: 0x00200A97
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkType = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x002026B3 File Offset: 0x00200AB3
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs();
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x002026CC File Offset: 0x00200ACC
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
