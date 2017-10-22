using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompMannable : ThingComp
	{
		private int lastManTick = -1;

		private Pawn lastManPawn = null;

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
				return this.MannedNow ? this.lastManPawn : null;
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
			_003CCompFloatMenuOptions_003Ec__Iterator0 _003CCompFloatMenuOptions_003Ec__Iterator = (_003CCompFloatMenuOptions_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			if (pawn.RaceProps.ToolUser && pawn.CanReserveAndReach((Thing)base.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false) && (this.Props.manWorkType == WorkTags.None || pawn.story == null || !pawn.story.WorkTagIsDisabled(this.Props.manWorkType)))
			{
				FloatMenuOption opt = new FloatMenuOption("OrderManThing".Translate(base.parent.LabelShort), (Action)delegate()
				{
					Job job = new Job(JobDefOf.ManTurret, (Thing)_003CCompFloatMenuOptions_003Ec__Iterator._0024this.parent);
					pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return opt;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
