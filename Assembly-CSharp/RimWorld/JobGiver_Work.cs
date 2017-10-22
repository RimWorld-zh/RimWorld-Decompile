using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Work : ThinkNode
	{
		public bool emergency;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		public override float GetPriority(Pawn pawn)
		{
			if (pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
				if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
				{
					return 5.5f;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Work)
				{
					return 9f;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
				{
					return 2f;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
				{
					return 2f;
				}
				throw new NotImplementedException();
			}
			return 0f;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.emergency && pawn.mindState.priorityWork.IsPrioritized)
			{
				List<WorkGiverDef> workGiversByPriority = pawn.mindState.priorityWork.WorkType.workGiversByPriority;
				for (int i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					Job job = this.GiverTryGiveJobPrioritized(pawn, worker, pawn.mindState.priorityWork.Cell);
					if (job != null)
					{
						job.playerForced = true;
						return new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive));
					}
				}
				pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = this.emergency ? pawn.workSettings.WorkGiversInOrderEmergency : pawn.workSettings.WorkGiversInOrderNormal;
			int num = -999;
			TargetInfo targetInfo = TargetInfo.Invalid;
			WorkGiver_Scanner workGiver_Scanner = null;
			WorkGiver_Scanner scanner;
			for (int j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && targetInfo.IsValid)
					break;
				if (this.PawnCanUseWorkGiver(pawn, workGiver))
				{
					try
					{
						Job job2 = workGiver.NonScanJob(pawn);
						if (job2 != null)
						{
							return new ThinkResult(job2, this, new JobTag?(list[j].def.tagToGive));
						}
						scanner = (workGiver as WorkGiver_Scanner);
						if (scanner != null)
						{
							if (workGiver.def.scanThings)
							{
								Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false));
								IEnumerable<Thing> enumerable = scanner.PotentialWorkThingsGlobal(pawn);
								Thing thing;
								if (scanner.Prioritized)
								{
									IEnumerable<Thing> enumerable2 = enumerable;
									if (enumerable2 == null)
									{
										enumerable2 = pawn.Map.listerThings.ThingsMatching(scanner.PotentialWorkThingRequest);
									}
									Predicate<Thing> validator = predicate;
									thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, enumerable2, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, (Func<Thing, float>)((Thing x) => scanner.GetPriority(pawn, x)));
								}
								else
								{
									Predicate<Thing> validator = predicate;
									bool forceGlobalSearch = enumerable != null;
									thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, scanner.PotentialWorkThingRequest, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, enumerable, 0, scanner.LocalRegionsToScanFirst, forceGlobalSearch, RegionType.Set_Passable, false);
								}
								if (thing != null)
								{
									targetInfo = thing;
									workGiver_Scanner = scanner;
								}
							}
							if (workGiver.def.scanCells)
							{
								IntVec3 position = pawn.Position;
								float num2 = 99999f;
								float num3 = -3.40282347E+38f;
								bool prioritized = scanner.Prioritized;
								foreach (IntVec3 item in scanner.PotentialWorkCellsGlobal(pawn))
								{
									bool flag = false;
									float num4 = (float)(item - position).LengthHorizontalSquared;
									if (prioritized)
									{
										if (!item.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, item))
										{
											float priority = scanner.GetPriority(pawn, item);
											if (priority > num3 || (priority == num3 && num4 < num2))
											{
												flag = true;
												num3 = priority;
											}
										}
									}
									else if (num4 < num2 && !item.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, item))
									{
										flag = true;
									}
									if (flag)
									{
										targetInfo = new TargetInfo(item, pawn.Map, false);
										workGiver_Scanner = scanner;
										num2 = num4;
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error(pawn + " threw exception in WorkGiver " + workGiver.def.defName + ": " + ex.ToString());
					}
					finally
					{
					}
					if (targetInfo.IsValid)
					{
						pawn.mindState.lastGivenWorkType = workGiver.def.workType;
						Job job3 = (!targetInfo.HasThing) ? workGiver_Scanner.JobOnCell(pawn, targetInfo.Cell) : workGiver_Scanner.JobOnThing(pawn, targetInfo.Thing, false);
						if (job3 == null)
						{
							Log.ErrorOnce(workGiver_Scanner + " provided target " + targetInfo + " but yielded no actual job for pawn " + pawn + ". The CanGiveJob and JobOnX methods may not be synchronized.", 6112651);
							goto IL_05c2;
						}
						return new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive));
					}
					goto IL_05c2;
				}
				continue;
				IL_05c2:
				num = workGiver.def.priorityInType;
			}
			return ThinkResult.NoJob;
		}

		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			if (giver.ShouldSkip(pawn))
			{
				return false;
			}
			if (!giver.def.canBeDoneByNonColonists && !pawn.IsColonist)
			{
				return false;
			}
			if (pawn.story != null && pawn.story.WorkTagIsDisabled(giver.def.workTags))
			{
				return false;
			}
			if (giver.MissingRequiredCapacity(pawn) != null)
			{
				return false;
			}
			return true;
		}

		private Job GiverTryGiveJobPrioritized(Pawn pawn, WorkGiver giver, IntVec3 cell)
		{
			if (!this.PawnCanUseWorkGiver(pawn, giver))
			{
				return null;
			}
			try
			{
				Job job = giver.NonScanJob(pawn);
				if (job != null)
				{
					return job;
				}
				WorkGiver_Scanner scanner = giver as WorkGiver_Scanner;
				if (scanner != null)
				{
					if (giver.def.scanThings)
					{
						Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false));
						List<Thing> thingList = cell.GetThingList(pawn.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (scanner.PotentialWorkThingRequest.Accepts(thing) && predicate(thing))
							{
								pawn.mindState.lastGivenWorkType = giver.def.workType;
								return scanner.JobOnThing(pawn, thing, false);
							}
						}
					}
					if (giver.def.scanCells && !cell.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, cell))
					{
						pawn.mindState.lastGivenWorkType = giver.def.workType;
						return scanner.JobOnCell(pawn, cell);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(pawn + " threw exception in GiverTryGiveJobTargeted on WorkGiver " + giver.def.defName + ": " + ex.ToString());
			}
			return null;
		}
	}
}
