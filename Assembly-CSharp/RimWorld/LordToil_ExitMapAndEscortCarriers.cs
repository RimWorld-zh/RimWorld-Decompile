using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000190 RID: 400
	public class LordToil_ExitMapAndEscortCarriers : LordToil
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0004F5B0 File Offset: 0x0004D9B0
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0004F5C8 File Offset: 0x0004D9C8
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0004F5E0 File Offset: 0x0004D9E0
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

		// Token: 0x06000846 RID: 2118 RVA: 0x0004F654 File Offset: 0x0004DA54
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

		// Token: 0x06000847 RID: 2119 RVA: 0x0004F6B8 File Offset: 0x0004DAB8
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

		// Token: 0x06000848 RID: 2120 RVA: 0x0004F75C File Offset: 0x0004DB5C
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

		// Token: 0x06000849 RID: 2121 RVA: 0x0004F878 File Offset: 0x0004DC78
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
			bool result;
			if (thing3 == null)
			{
				result = false;
			}
			else if (thing3 is Pawn && !((Pawn)thing3).Downed)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.Escort, thing3, escortRadius);
				result = true;
			}
			else if (!GenHostility.AnyHostileActiveThreatTo(base.Map, this.lord.faction))
			{
				result = false;
			}
			else
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.Defend, thing3.Position, 16f);
				result = true;
			}
			return result;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0004FA74 File Offset: 0x0004DE74
		public static bool IsDefendingPosition(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Defend;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0004FAB4 File Offset: 0x0004DEB4
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

		// Token: 0x0600084C RID: 2124 RVA: 0x0004FAFC File Offset: 0x0004DEFC
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
	}
}
