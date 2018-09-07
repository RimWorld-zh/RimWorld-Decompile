using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_Ingest : JoyGiver
	{
		private static List<Thing> tmpCandidates = new List<Thing>();

		public JoyGiver_Ingest()
		{
		}

		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map));
		}

		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			if (thing != null)
			{
				return this.CreateIngestJob(thing, pawn);
			}
			return null;
		}

		protected virtual Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Predicate<Thing> predicate = (Thing t) => this.CanIngestForJoy(pawn, t) && (extraValidator == null || extraValidator(t));
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.SearchSetWouldInclude(innerContainer[i]) && predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_Ingest.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_Ingest.tmpCandidates);
			if (JoyGiver_Ingest.tmpCandidates.Count == 0)
			{
				return null;
			}
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			List<Thing> searchSet = JoyGiver_Ingest.tmpCandidates;
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			Thing result = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, null);
			JoyGiver_Ingest.tmpCandidates.Clear();
			return result;
		}

		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0f)
			{
				return false;
			}
			if (t.Spawned)
			{
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (!t.IsPoliticallyProper(pawn))
				{
					return false;
				}
			}
			if (t.def.IsDrug && pawn.drugs != null && !pawn.drugs.CurrentPolicy[t.def].allowedForJoy && pawn.story != null)
			{
				int num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
				if (num <= 0 && !pawn.InMentalState)
				{
					return false;
				}
			}
			return true;
		}

		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return new Job(JobDefOf.Ingest, ingestible)
			{
				count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce)
			};
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JoyGiver_Ingest()
		{
		}

		[CompilerGenerated]
		private sealed class <TryGiveJobInPartyArea>c__AnonStorey0
		{
			internal IntVec3 partySpot;

			internal Pawn pawn;

			public <TryGiveJobInPartyArea>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return !x.Spawned || PartyUtility.InPartyArea(x.Position, this.partySpot, this.pawn.Map);
			}
		}

		[CompilerGenerated]
		private sealed class <BestIngestItem>c__AnonStorey1
		{
			internal Pawn pawn;

			internal Predicate<Thing> extraValidator;

			internal JoyGiver_Ingest $this;

			public <BestIngestItem>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return this.$this.CanIngestForJoy(this.pawn, t) && (this.extraValidator == null || this.extraValidator(t));
			}
		}
	}
}
