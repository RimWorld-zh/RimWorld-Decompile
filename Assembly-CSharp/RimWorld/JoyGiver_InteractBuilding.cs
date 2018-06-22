using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F2 RID: 242
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00038984 File Offset: 0x00036D84
		protected virtual bool CanDoDuringParty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0003899C File Offset: 0x00036D9C
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			Job result;
			if (thing != null)
			{
				result = this.TryGivePlayJob(pawn, thing);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000389D4 File Offset: 0x00036DD4
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			Job result;
			if (thing != null)
			{
				result = this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00038A0C File Offset: 0x00036E0C
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
				if (thing != null)
				{
					result = this.TryGivePlayJob(pawn, thing);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00038A54 File Offset: 0x00036E54
		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 partySpot)
		{
			List<Thing> searchSet = this.GetSearchSet(pawn);
			Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
			if (partySpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = ((Thing x) => PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) && oldValidator(x));
			}
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			List<Thing> searchSet2 = searchSet;
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThing_Global_Reachable(position, map, searchSet2, peMode, traverseParams, 9999f, validator, null);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00038B28 File Offset: 0x00036F28
		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			bool result;
			if (!pawn.CanReserve(t, this.def.jobDef.joyMaxParticipants, -1, null, false))
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
				result = ((compPowerTrader == null || compPowerTrader.PowerOn) && (!this.def.unroofedOnly || !t.Position.Roofed(t.Map)));
			}
			return result;
		}

		// Token: 0x06000521 RID: 1313
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x06000522 RID: 1314 RVA: 0x00038BF0 File Offset: 0x00036FF0
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return new Job(this.def.jobDef, bestGame, pawn.Position, t);
		}
	}
}
