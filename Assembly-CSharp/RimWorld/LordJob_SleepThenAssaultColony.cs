using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		private Faction faction;

		private bool wakeUpIfColonistClose;

		private const int AnyColonistCloseCheckIntervalTicks = 30;

		private const float AnyColonistCloseCheckRadius = 6f;

		public LordJob_SleepThenAssaultColony()
		{
		}

		public LordJob_SleepThenAssaultColony(Faction faction, bool wakeUpIfColonistClose)
		{
			this.faction = faction;
			this.wakeUpIfColonistClose = wakeUpIfColonistClose;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig));
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition);
			if (this.wakeUpIfColonistClose)
			{
				transition.AddTrigger(new Trigger_Custom((Func<TriggerSignal, bool>)((TriggerSignal x) => Find.TickManager.TicksGame % 30 == 0 && this.AnyColonistClose())));
			}
			return stateGraph;
		}

		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
		}

		private bool AnyColonistClose()
		{
			int num = GenRadial.NumCellsInRadius(6f);
			Map map = base.Map;
			int num2 = 0;
			bool result;
			while (true)
			{
				if (num2 < base.lord.ownedPawns.Count)
				{
					Pawn pawn = base.lord.ownedPawns[num2];
					for (int num3 = 0; num3 < num; num3++)
					{
						IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num3];
						if (intVec.InBounds(map) && this.AnyColonistAt(intVec) && GenSight.LineOfSight(pawn.Position, intVec, map, false, null, 0, 0))
							goto IL_0085;
					}
					num2++;
					continue;
				}
				result = false;
				break;
				IL_0085:
				result = true;
				break;
			}
			return result;
		}

		private bool AnyColonistAt(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Pawn pawn = thingList[num] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
