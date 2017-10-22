using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_Ingest : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return this.TryGiveJobInternal(pawn, (Predicate<Thing>)((Thing x) => !x.Spawned || PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map)));
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
			Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing t)
			{
				if (!this.CanIngestForJoy(pawn, t))
				{
					return false;
				}
				if ((object)extraValidator != null && !extraValidator(t))
				{
					return false;
				}
				return true;
			};
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.SearchSetWouldInclude(innerContainer[i]) && predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			List<Thing> searchSet = this.GetSearchSet(pawn);
			if (searchSet.Count == 0)
			{
				return null;
			}
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, searchSet, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
		}

		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			if (t.def.IsIngestible && t.def.ingestible.joyKind != null && !(t.def.ingestible.joy <= 0.0))
			{
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
			return false;
		}

		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			if (base.def.thingDefs == null)
			{
				return false;
			}
			return base.def.thingDefs.Contains(thing.def);
		}

		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = new Job(JobDefOf.Ingest, ingestible);
			job.count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce);
			return job;
		}
	}
}
