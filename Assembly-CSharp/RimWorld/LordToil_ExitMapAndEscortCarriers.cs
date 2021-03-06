﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_ExitMapAndEscortCarriers : LordToil
	{
		public LordToil_ExitMapAndEscortCarriers()
		{
		}

		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		public override void UpdateAllDuties()
		{
			Pawn trader;
			this.UpdateTraderDuty(out trader);
			this.UpdateCarriersDuties(trader);
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn p = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
				if (traderCaravanRole != TraderCaravanRole.Carrier && traderCaravanRole != TraderCaravanRole.Trader)
				{
					this.UpdateDutyForChattelOrGuard(p, trader);
				}
			}
		}

		private void UpdateTraderDuty(out Pawn trader)
		{
			trader = TraderCaravanUtility.FindTrader(this.lord);
			if (trader != null)
			{
				trader.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				trader.mindState.duty.radius = 18f;
				trader.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		private void UpdateCarriersDuties(Pawn trader)
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					if (trader != null)
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.Follow, trader, 5f);
					}
					else
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
						pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
					}
				}
			}
		}

		private void UpdateDutyForChattelOrGuard(Pawn p, Pawn trader)
		{
			TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
			if (traderCaravanRole == TraderCaravanRole.Chattel)
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 14f);
				}
				else if (!this.TryToDefendClosestCarrier(p, 14f))
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
					p.mindState.duty.radius = 10f;
					p.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
			}
			else if (!this.TryToDefendClosestCarrier(p, 26f))
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 26f);
				}
				else
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
					p.mindState.duty.radius = 18f;
					p.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
			}
		}

		private bool TryToDefendClosestCarrier(Pawn p, float escortRadius)
		{
			Pawn closestCarrier = this.GetClosestCarrier(p);
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 20f, delegate(Thing x)
			{
				Pawn innerPawn = ((Corpse)x).InnerPawn;
				return innerPawn.Faction == p.Faction && innerPawn.RaceProps.packAnimal;
			}, null, 0, 15, false, RegionType.Set_Passable, false);
			Thing thing2 = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 20f, delegate(Thing x)
			{
				Pawn pawn = (Pawn)x;
				return pawn.Downed && pawn.Faction == p.Faction && pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
			}, null, 0, 15, false, RegionType.Set_Passable, false);
			Thing thing3 = null;
			if (closestCarrier != null)
			{
				thing3 = closestCarrier;
			}
			if (thing != null && (thing3 == null || thing.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing;
			}
			if (thing2 != null && (thing3 == null || thing2.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing2;
			}
			if (thing3 == null)
			{
				return false;
			}
			if (thing3 is Pawn && !((Pawn)thing3).Downed)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.Escort, thing3, escortRadius);
				return true;
			}
			if (!GenHostility.AnyHostileActiveThreatTo(base.Map, this.lord.faction))
			{
				return false;
			}
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, thing3.Position, 16f);
			return true;
		}

		public static bool IsDefendingPosition(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Defend;
		}

		public static bool IsAnyDefendingPosition(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (LordToil_ExitMapAndEscortCarriers.IsDefendingPosition(pawns[i]))
				{
					return true;
				}
			}
			return false;
		}

		private Pawn GetClosestCarrier(Pawn closestTo)
		{
			Pawn pawn = null;
			float num = 0f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = this.lord.ownedPawns[i];
				if (pawn2.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(closestTo.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}

		[CompilerGenerated]
		private sealed class <TryToDefendClosestCarrier>c__AnonStorey0
		{
			internal Pawn p;

			public <TryToDefendClosestCarrier>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				Pawn innerPawn = ((Corpse)x).InnerPawn;
				return innerPawn.Faction == this.p.Faction && innerPawn.RaceProps.packAnimal;
			}

			internal bool <>m__1(Thing x)
			{
				Pawn pawn = (Pawn)x;
				return pawn.Downed && pawn.Faction == this.p.Faction && pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
			}
		}
	}
}
