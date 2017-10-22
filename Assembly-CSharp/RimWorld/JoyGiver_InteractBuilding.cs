using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		protected virtual bool CanDoDuringParty
		{
			get
			{
				return false;
			}
		}

		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			return (thing == null) ? null : this.TryGivePlayJob(pawn, thing);
		}

		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			return (thing == null) ? null : this.TryGivePlayJobWhileInBed(pawn, thing);
		}

		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			Job result;
			if (!this.CanDoDuringParty)
			{
				result = null;
			}
			else
			{
				Thing thing = this.FindBestGame(pawn, false, partySpot);
				result = ((thing == null) ? null : this.TryGivePlayJob(pawn, thing));
			}
			return result;
		}

		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 partySpot)
		{
			List<Thing> searchSet = this.GetSearchSet(pawn);
			Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => this.CanInteractWith(pawn, t, inBed));
			if (partySpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = (Predicate<Thing>)((Thing x) => PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) && oldValidator(x));
			}
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			List<Thing> searchSet2 = searchSet;
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThing_Global_Reachable(position, map, searchSet2, peMode, traverseParams, 9999f, validator, null);
		}

		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			bool result;
			if (!pawn.CanReserve(t, base.def.jobDef.joyMaxParticipants, -1, null, false))
			{
				result = false;
			}
			else if (t.IsForbidden(pawn))
			{
				result = false;
			}
			else if (!t.IsSociallyProper(pawn))
			{
				result = false;
			}
			else if (!t.IsPoliticallyProper(pawn))
			{
				result = false;
			}
			else
			{
				CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
				result = ((byte)((compPowerTrader == null || compPowerTrader.PowerOn) ? ((!base.def.unroofedOnly || !t.Position.Roofed(t.Map)) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return new Job(base.def.jobDef, bestGame, pawn.Position, (Thing)t);
		}
	}
}
