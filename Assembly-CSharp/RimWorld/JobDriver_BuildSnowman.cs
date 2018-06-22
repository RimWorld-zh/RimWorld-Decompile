using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000053 RID: 83
	public class JobDriver_BuildSnowman : JobDriver
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0001B438 File Offset: 0x00019838
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001B46C File Offset: 0x0001986C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 2300f;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (this.workLeft <= 0f)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
					thing.SetFaction(this.pawn.Faction, null);
					GenSpawn.Spawn(thing, this.TargetLocA, this.Map, WipeMode.Vanish);
					this.ReadyForNextToil();
				}
				else
				{
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return doWork;
			yield break;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0001B496 File Offset: 0x00019896
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x040001F1 RID: 497
		private float workLeft = -1000f;

		// Token: 0x040001F2 RID: 498
		protected const int BaseWorkAmount = 2300;
	}
}
