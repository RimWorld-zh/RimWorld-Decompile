using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		private static List<Thing> tmpCandidates = new List<Thing>();

		protected JoyGiver_InteractBuilding()
		{
		}

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

		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 partySpot)
		{
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_InteractBuilding.tmpCandidates);
			Thing result;
			if (JoyGiver_InteractBuilding.tmpCandidates.Count == 0)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
				if (partySpot.IsValid)
				{
					Predicate<Thing> oldValidator = predicate;
					predicate = ((Thing x) => PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) && oldValidator(x));
				}
				IntVec3 position = pawn.Position;
				Map map = pawn.Map;
				List<Thing> searchSet = JoyGiver_InteractBuilding.tmpCandidates;
				PathEndMode peMode = PathEndMode.OnCell;
				TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
				Predicate<Thing> validator = predicate;
				Thing thing = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, null);
				JoyGiver_InteractBuilding.tmpCandidates.Clear();
				result = thing;
			}
			return result;
		}

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

		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return new Job(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JoyGiver_InteractBuilding()
		{
		}

		[CompilerGenerated]
		private sealed class <FindBestGame>c__AnonStorey0
		{
			internal Pawn pawn;

			internal bool inBed;

			internal IntVec3 partySpot;

			internal JoyGiver_InteractBuilding $this;

			public <FindBestGame>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return this.$this.CanInteractWith(this.pawn, t, this.inBed);
			}
		}

		[CompilerGenerated]
		private sealed class <FindBestGame>c__AnonStorey1
		{
			internal Predicate<Thing> oldValidator;

			internal JoyGiver_InteractBuilding.<FindBestGame>c__AnonStorey0 <>f__ref$0;

			public <FindBestGame>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return PartyUtility.InPartyArea(x.Position, this.<>f__ref$0.partySpot, this.<>f__ref$0.pawn.Map) && this.oldValidator(x);
			}
		}
	}
}
