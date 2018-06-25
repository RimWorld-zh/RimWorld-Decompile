using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		public WorkGiver_Merge()
		{
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (t.stackCount == t.def.stackLimit)
			{
				result = null;
			}
			else if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				result = null;
			}
			else
			{
				SlotGroup slotGroup = t.GetSlotGroup();
				if (slotGroup == null)
				{
					result = null;
				}
				else
				{
					foreach (Thing thing in slotGroup.HeldThings)
					{
						if (thing != t)
						{
							if (thing.def == t.def)
							{
								if (forced || thing.stackCount >= t.stackCount)
								{
									if (thing.stackCount != thing.def.stackLimit)
									{
										LocalTargetInfo target = thing.Position;
										if (pawn.CanReserve(target, 1, -1, null, forced))
										{
											if (thing.Position.IsValidStorageFor(thing.Map, t))
											{
												return new Job(JobDefOf.HaulToCell, t, thing.Position)
												{
													count = Mathf.Min(thing.def.stackLimit - thing.stackCount, t.stackCount),
													haulMode = HaulMode.ToCellStorage
												};
											}
										}
									}
								}
							}
						}
					}
					JobFailReason.Is("NoMergeTarget".Translate(), null);
					result = null;
				}
			}
			return result;
		}
	}
}
