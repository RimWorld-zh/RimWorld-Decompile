using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FF RID: 255
	public class JoyGiver_Ingest : JoyGiver
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x0003A33C File Offset: 0x0003873C
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0003A35C File Offset: 0x0003875C
		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map));
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0003A3A0 File Offset: 0x000387A0
		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			Job result;
			if (thing != null)
			{
				result = this.CreateIngestJob(thing, pawn);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0003A3D4 File Offset: 0x000387D4
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
			List<Thing> searchSet = this.GetSearchSet(pawn);
			if (searchSet.Count == 0)
			{
				return null;
			}
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			List<Thing> searchSet2 = searchSet;
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThing_Global_Reachable(position, map, searchSet2, peMode, traverseParams, 9999f, validator, null);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0003A4DC File Offset: 0x000388DC
		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			bool result;
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0f)
			{
				result = false;
			}
			else
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
				result = true;
			}
			return result;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0003A614 File Offset: 0x00038A14
		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0003A658 File Offset: 0x00038A58
		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return new Job(JobDefOf.Ingest, ingestible)
			{
				count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce)
			};
		}
	}
}
