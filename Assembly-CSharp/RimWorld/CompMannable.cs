using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class CompMannable : ThingComp
	{
		// Token: 0x040015FD RID: 5629
		private int lastManTick = -1;

		// Token: 0x040015FE RID: 5630
		private Pawn lastManPawn = null;

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x001579DC File Offset: 0x00155DDC
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x00157A24 File Offset: 0x00155E24
		public Pawn ManningPawn
		{
			get
			{
				Pawn result;
				if (!this.MannedNow)
				{
					result = null;
				}
				else
				{
					result = this.lastManPawn;
				}
				return result;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x00157A54 File Offset: 0x00155E54
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x00157A74 File Offset: 0x00155E74
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x00157AA0 File Offset: 0x00155EA0
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (!pawn.RaceProps.ToolUser)
			{
				yield break;
			}
			if (!pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
			{
				yield break;
			}
			if (this.Props.manWorkType != WorkTags.None && pawn.story != null && pawn.story.WorkTagIsDisabled(this.Props.manWorkType))
			{
				if (this.Props.manWorkType == WorkTags.Violent)
				{
					yield return new FloatMenuOption("CannotManThing".Translate(new object[]
					{
						this.parent.LabelShort
					}) + " (" + "IsIncapableOfViolenceLower".Translate(new object[]
					{
						pawn.LabelShort
					}) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				yield break;
			}
			FloatMenuOption opt = new FloatMenuOption("OrderManThing".Translate(new object[]
			{
				this.parent.LabelShort
			}), delegate()
			{
				Job job = new Job(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield return opt;
			yield break;
		}
	}
}
