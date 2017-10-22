#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Work : ThinkNode
	{
		public bool emergency = false;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
				if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
				{
					result = 5.5f;
					goto IL_00a6;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Work)
				{
					result = 9f;
					goto IL_00a6;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
				{
					result = 2f;
					goto IL_00a6;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
				{
					result = 2f;
					goto IL_00a6;
				}
				throw new NotImplementedException();
			}
			result = 0f;
			goto IL_00a6;
			IL_00a6:
			return result;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Profiler.BeginSample("JobGiver_Work");
			List<WorkGiverDef> workGiversByPriority;
			int i;
			Job job;
			if (this.emergency && pawn.mindState.priorityWork.IsPrioritized)
			{
				workGiversByPriority = pawn.mindState.priorityWork.WorkType.workGiversByPriority;
				for (i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					job = this.GiverTryGiveJobPrioritized(pawn, worker, pawn.mindState.priorityWork.Cell);
					if (job != null)
						goto IL_0099;
				}
				pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = this.emergency ? pawn.workSettings.WorkGiversInOrderEmergency : pawn.workSettings.WorkGiversInOrderNormal;
			int num = -999;
			TargetInfo targetInfo = TargetInfo.Invalid;
			WorkGiver_Scanner workGiver_Scanner = null;
			int j;
			WorkGiver_Scanner scanner;
			Job job3;
			for (j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && targetInfo.IsValid)
					break;
				if (this.PawnCanUseWorkGiver(pawn, workGiver))
				{
					Profiler.BeginSample("WorkGiver: " + workGiver.def.defName);
					try
					{
						Job job2 = workGiver.NonScanJob(pawn);
						if (job2 != null)
						{
							return new ThinkResult(job2, this, new JobTag?(list[j].def.tagToGive), false);
						}
						scanner = (workGiver as WorkGiver_Scanner);
						if (scanner != null)
						{
							IntVec3 intVec;
							if (scanner.def.scanThings)
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
									if (scanner.AllowUnreachable)
									{
										intVec = pawn.Position;
										IEnumerable<Thing> searchSet = enumerable2;
										Predicate<Thing> validator = predicate;
										thing = GenClosest.ClosestThing_Global(intVec, searchSet, 99999f, validator, (Func<Thing, float>)((Thing x) => scanner.GetPriority(pawn, x)));
									}
									else
									{
										intVec = pawn.Position;
										Map map = pawn.Map;
										IEnumerable<Thing> searchSet = enumerable2;
										PathEndMode pathEndMode = scanner.PathEndMode;
										TraverseParms traverseParams = TraverseParms.For(pawn, scanner.MaxPathDanger(pawn), TraverseMode.ByPawn, false);
										Predicate<Thing> validator = predicate;
										thing = GenClosest.ClosestThing_Global_Reachable(intVec, map, searchSet, pathEndMode, traverseParams, 9999f, validator, (Func<Thing, float>)((Thing x) => scanner.GetPriority(pawn, x)));
									}
								}
								else if (scanner.AllowUnreachable)
								{
									IEnumerable<Thing> enumerable3 = enumerable;
									if (enumerable3 == null)
									{
										enumerable3 = pawn.Map.listerThings.ThingsMatching(scanner.PotentialWorkThingRequest);
									}
									intVec = pawn.Position;
									IEnumerable<Thing> searchSet = enumerable3;
									Predicate<Thing> validator = predicate;
									thing = GenClosest.ClosestThing_Global(intVec, searchSet, 99999f, validator, null);
								}
								else
								{
									intVec = pawn.Position;
									Map map = pawn.Map;
									ThingRequest potentialWorkThingRequest = scanner.PotentialWorkThingRequest;
									PathEndMode pathEndMode = scanner.PathEndMode;
									TraverseParms traverseParams = TraverseParms.For(pawn, scanner.MaxPathDanger(pawn), TraverseMode.ByPawn, false);
									Predicate<Thing> validator = predicate;
									bool forceGlobalSearch = enumerable != null;
									thing = GenClosest.ClosestThingReachable(intVec, map, potentialWorkThingRequest, pathEndMode, traverseParams, 9999f, validator, enumerable, 0, scanner.LocalRegionsToScanFirst, forceGlobalSearch, RegionType.Set_Passable, false);
								}
								if (thing != null)
								{
									targetInfo = thing;
									workGiver_Scanner = scanner;
								}
							}
							if (scanner.def.scanCells)
							{
								IntVec3 position = pawn.Position;
								float num2 = 99999f;
								float num3 = -3.40282347E+38f;
								bool prioritized = scanner.Prioritized;
								bool allowUnreachable = scanner.AllowUnreachable;
								Danger maxDanger = scanner.MaxPathDanger(pawn);
								foreach (IntVec3 item in scanner.PotentialWorkCellsGlobal(pawn))
								{
									bool flag = false;
									intVec = item - position;
									float num4 = (float)intVec.LengthHorizontalSquared;
									float num5 = 0f;
									if (prioritized)
									{
										if (scanner.HasJobOnCell(pawn, item))
										{
											if (!allowUnreachable && !pawn.CanReach(item, scanner.PathEndMode, maxDanger, false, TraverseMode.ByPawn))
											{
												continue;
											}
											num5 = scanner.GetPriority(pawn, item);
											if (num5 > num3 || (num5 == num3 && num4 < num2))
											{
												flag = true;
											}
										}
									}
									else if (num4 < num2 && scanner.HasJobOnCell(pawn, item))
									{
										if (!allowUnreachable && !pawn.CanReach(item, scanner.PathEndMode, maxDanger, false, TraverseMode.ByPawn))
										{
											continue;
										}
										flag = true;
									}
									if (flag)
									{
										targetInfo = new TargetInfo(item, pawn.Map, false);
										workGiver_Scanner = scanner;
										num2 = num4;
										num3 = num5;
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
						Profiler.EndSample();
					}
					if (targetInfo.IsValid)
					{
						Profiler.EndSample();
						pawn.mindState.lastGivenWorkType = workGiver.def.workType;
						job3 = ((!targetInfo.HasThing) ? workGiver_Scanner.JobOnCell(pawn, targetInfo.Cell) : workGiver_Scanner.JobOnThing(pawn, targetInfo.Thing, false));
						if (job3 == null)
						{
							Log.ErrorOnce(workGiver_Scanner + " provided target " + targetInfo + " but yielded no actual job for pawn " + pawn + ". The CanGiveJob and JobOnX methods may not be synchronized.", 6112651);
							goto IL_078d;
						}
						goto IL_0764;
					}
					goto IL_078d;
				}
				continue;
				IL_078d:
				num = workGiver.def.priorityInType;
			}
			Profiler.EndSample();
			ThinkResult result = ThinkResult.NoJob;
			goto IL_07c1;
			IL_07c1:
			return result;
			IL_0099:
			Profiler.EndSample();
			job.playerForced = true;
			result = new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive), false);
			goto IL_07c1;
			IL_0764:
			result = new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive), false);
			goto IL_07c1;
		}

		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			return (byte)((!giver.ShouldSkip(pawn)) ? ((giver.def.canBeDoneByNonColonists || pawn.IsColonist) ? ((pawn.story == null || !pawn.story.WorkTagIsDisabled(giver.def.workTags)) ? ((giver.MissingRequiredCapacity(pawn) == null) ? 1 : 0) : 0) : 0) : 0) != 0;
		}

		private Job GiverTryGiveJobPrioritized(Pawn pawn, WorkGiver giver, IntVec3 cell)
		{
			Job result;
			if (!this.PawnCanUseWorkGiver(pawn, giver))
			{
				result = null;
			}
			else
			{
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
				result = null;
			}
			return result;
		}
	}
}
