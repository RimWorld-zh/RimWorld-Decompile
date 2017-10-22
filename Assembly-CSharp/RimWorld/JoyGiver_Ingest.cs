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
			return (thing == null) ? null : this.CreateIngestJob(thing, pawn);
		}

		protected virtual Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => (byte)(this.CanIngestForJoy(pawn, t) ? (((object)extraValidator == null || extraValidator(t)) ? 1 : 0) : 0) != 0);
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < innerContainer.Count)
				{
					if (this.SearchSetWouldInclude(innerContainer[num]) && predicate(innerContainer[num]))
					{
						result = innerContainer[num];
						break;
					}
					num++;
					continue;
				}
				List<Thing> searchSet = this.GetSearchSet(pawn);
				if (searchSet.Count == 0)
				{
					result = null;
				}
				else
				{
					IntVec3 position = pawn.Position;
					Map map = pawn.Map;
					List<Thing> searchSet2 = searchSet;
					PathEndMode peMode = PathEndMode.OnCell;
					TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
					Predicate<Thing> validator = predicate;
					result = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet2, peMode, traverseParams, 9999f, validator, null);
				}
				break;
			}
			return result;
		}

		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			bool result;
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0.0)
			{
				result = false;
			}
			else
			{
				if (t.Spawned)
				{
					if (!pawn.CanReserve(t, 1, -1, null, false))
					{
						result = false;
						goto IL_0127;
					}
					if (t.IsForbidden(pawn))
					{
						result = false;
						goto IL_0127;
					}
					if (!t.IsSociallyProper(pawn))
					{
						result = false;
						goto IL_0127;
					}
					if (!t.IsPoliticallyProper(pawn))
					{
						result = false;
						goto IL_0127;
					}
				}
				if (t.def.IsDrug && pawn.drugs != null && !pawn.drugs.CurrentPolicy[t.def].allowedForJoy && pawn.story != null)
				{
					int num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
					if (num <= 0 && !pawn.InMentalState)
					{
						result = false;
						goto IL_0127;
					}
				}
				result = true;
			}
			goto IL_0127;
			IL_0127:
			return result;
		}

		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return base.def.thingDefs != null && base.def.thingDefs.Contains(thing.def);
		}

		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = new Job(JobDefOf.Ingest, ingestible);
			job.count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce);
			return job;
		}
	}
}
