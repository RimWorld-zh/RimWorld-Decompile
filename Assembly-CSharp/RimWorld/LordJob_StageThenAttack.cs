using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000176 RID: 374
	public class LordJob_StageThenAttack : LordJob
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x0004B923 File Offset: 0x00049D23
		public LordJob_StageThenAttack()
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0004B92C File Offset: 0x00049D2C
		public LordJob_StageThenAttack(Faction faction, IntVec3 stageLoc, int raidSeed)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.raidSeed = raidSeed;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0004B94C File Offset: 0x00049D4C
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0004B964 File Offset: 0x00049D64
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			int tickLimit = Rand.RangeSeeded(5000, 15000, this.raidSeed);
			Transition transition = new Transition(lordToil_Stage, startingToil, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(tickLimit));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(new object[]
			{
				this.faction.def.pawnsPlural.CapitalizeFirst(),
				this.faction.Name
			}), MessageTypeDefOf.ThreatBig, "MessageRaidersBeginningAssault-" + this.raidSeed, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition);
			return stateGraph;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0004BA6C File Offset: 0x00049E6C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.raidSeed, "raidSeed", 0, false);
		}

		// Token: 0x0400035E RID: 862
		private Faction faction;

		// Token: 0x0400035F RID: 863
		private IntVec3 stageLoc;

		// Token: 0x04000360 RID: 864
		private int raidSeed;
	}
}
