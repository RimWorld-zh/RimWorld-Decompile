using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000175 RID: 373
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x0004B66F File Offset: 0x00049A6F
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0004B678 File Offset: 0x00049A78
		public LordJob_SleepThenAssaultColony(Faction faction, bool wakeUpIfColonistClose)
		{
			this.faction = faction;
			this.wakeUpIfColonistClose = wakeUpIfColonistClose;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0004B690 File Offset: 0x00049A90
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0004B6A8 File Offset: 0x00049AA8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(new object[]
			{
				this.faction.def.pawnsPlural
			}).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition);
			if (this.wakeUpIfColonistClose)
			{
				transition.AddTrigger(new Trigger_Custom((TriggerSignal x) => Find.TickManager.TicksGame % 30 == 0 && this.AnyColonistClose()));
			}
			return stateGraph;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0004B784 File Offset: 0x00049B84
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0004B7AC File Offset: 0x00049BAC
		private bool AnyColonistClose()
		{
			int num = GenRadial.NumCellsInRadius(6f);
			Map map = base.Map;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				for (int j = 0; j < num; j++)
				{
					IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[j];
					if (intVec.InBounds(map) && this.AnyColonistAt(intVec) && GenSight.LineOfSight(pawn.Position, intVec, map, false, null, 0, 0))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0004B87C File Offset: 0x00049C7C
		private bool AnyColonistAt(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400035A RID: 858
		private Faction faction;

		// Token: 0x0400035B RID: 859
		private bool wakeUpIfColonistClose;

		// Token: 0x0400035C RID: 860
		private const int AnyColonistCloseCheckIntervalTicks = 30;

		// Token: 0x0400035D RID: 861
		private const float AnyColonistCloseCheckRadius = 6f;
	}
}
