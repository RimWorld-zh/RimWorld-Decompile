using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompMannable : ThingComp
	{
		private int lastManTick = -1;

		private Pawn lastManPawn;

		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)base.props;
			}
		}

		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = base.parent;
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (pawn.RaceProps.ToolUser && pawn.CanReserveAndReach((Thing)base.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false) && (this.Props.manWorkType == WorkTags.None || pawn.story == null || !pawn.story.WorkTagIsDisabled(this.Props.manWorkType)))
			{
				FloatMenuOption opt = new FloatMenuOption("OrderManThing".Translate(base.parent.LabelShort), (Action)delegate
				{
					Job job = new Job(JobDefOf.ManTurret, (Thing)((_003CCompFloatMenuOptions_003Ec__Iterator168)/*Error near IL_00d9: stateMachine*/)._003C_003Ef__this.parent);
					((_003CCompFloatMenuOptions_003Ec__Iterator168)/*Error near IL_00d9: stateMachine*/).pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return opt;
			}
		}
	}
}
