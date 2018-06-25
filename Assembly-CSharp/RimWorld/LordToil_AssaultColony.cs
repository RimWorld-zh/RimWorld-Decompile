using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000187 RID: 391
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x0400037F RID: 895
		private bool attackDownedIfStarving;

		// Token: 0x0600081D RID: 2077 RVA: 0x0004E72E File Offset: 0x0004CB2E
		public LordToil_AssaultColony(bool attackDownedIfStarving = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0004E740 File Offset: 0x0004CB40
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0004E758 File Offset: 0x0004CB58
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0004E76E File Offset: 0x0004CB6E
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0004E784 File Offset: 0x0004CB84
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
			}
		}
	}
}
