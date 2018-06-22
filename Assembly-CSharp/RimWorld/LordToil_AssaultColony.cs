using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000187 RID: 391
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x0004E732 File Offset: 0x0004CB32
		public LordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0004E744 File Offset: 0x0004CB44
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0004E75C File Offset: 0x0004CB5C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0004E772 File Offset: 0x0004CB72
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0004E788 File Offset: 0x0004CB88
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
		}

		// Token: 0x0400037E RID: 894
		private bool attackDownedIfStarving;
	}
}
