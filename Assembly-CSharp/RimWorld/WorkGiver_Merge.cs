using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200014F RID: 335
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x00046B30 File Offset: 0x00044F30
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00046B48 File Offset: 0x00044F48
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00046B70 File Offset: 0x00044F70
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
