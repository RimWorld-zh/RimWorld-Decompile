using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000117 RID: 279
	public class JobGiver_Work : ThinkNode
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0003CF24 File Offset: 0x0003B324
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Work jobGiver_Work = (JobGiver_Work)base.DeepCopy(resolve);
			jobGiver_Work.emergency = this.emergency;
			return jobGiver_Work;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0003CF54 File Offset: 0x0003B354
		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				result = 0f;
			}
			else
			{
				TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
				if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
				{
					result = 5.5f;
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Work)
				{
					result = 9f;
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
				{
					result = 2f;
				}
				else
				{
					if (timeAssignmentDef != TimeAssignmentDefOf.Joy)
					{
						throw new NotImplementedException();
					}
					result = 2f;
				}
			}
			return result;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0003D008 File Offset: 0x0003B408
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Profiler.BeginSample("JobGiver_Work");
			if (this.emergency && pawn.mindState.priorityWork.IsPrioritized)
			{
				List<WorkGiverDef> workGiversByPriority = pawn.mindState.priorityWork.WorkType.workGiversByPriority;
				for (int i = 0; i < workGiversByPriority.Count; i++)
				{
					WorkGiver worker = workGiversByPriority[i].Worker;
					Job job = this.GiverTryGiveJobPrioritized(pawn, worker, pawn.mindState.priorityWork.Cell);
					if (job != null)
					{
						Profiler.EndSample();
						job.playerForced = true;
						return new ThinkResult(job, this, new JobTag?(workGiversByPriority[i].tagToGive), false);
					}
				}
				pawn.mindState.priorityWork.Clear();
			}
			List<WorkGiver> list = this.emergency ? pawn.workSettings.WorkGiversInOrderEmergency : pawn.workSettings.WorkGiversInOrderNormal;
			int num = -999;
			TargetInfo targetInfo = TargetInfo.Invalid;
			WorkGiver_Scanner workGiver_Scanner = null;
			for (int j = 0; j < list.Count; j++)
			{
				WorkGiver workGiver = list[j];
				if (workGiver.def.priorityInType != num && targetInfo.IsValid)
				{
					break;
				}
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
						WorkGiver_Scanner scanner = workGiver as WorkGiver_Scanner;
						if (scanner != null)
						{
							if (scanner.def.scanThings)
							{
								Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
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
										IntVec3 position = pawn.Position;
										IEnumerable<Thing> searchSet = enumerable2;
										Predicate<Thing> validator = predicate;
										thing = GenClosest.ClosestThing_Global(position, searchSet, 99999f, validator, (Thing x) => scanner.GetPriority(pawn, x));
									}
									else
									{
										IntVec3 position = pawn.Position;
										Map map = pawn.Map;
										IEnumerable<Thing> searchSet = enumerable2;
										PathEndMode pathEndMode = scanner.PathEndMode;
										TraverseParms traverseParams = TraverseParms.For(pawn, scanner.MaxPathDanger(pawn), TraverseMode.ByPawn, false);
										Predicate<Thing> validator = predicate;
										thing = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, pathEndMode, traverseParams, 9999f, validator, (Thing x) => scanner.GetPriority(pawn, x));
									}
								}
								else if (scanner.AllowUnreachable)
								{
									IEnumerable<Thing> enumerable3 = enumerable;
									if (enumerable3 == null)
									{
										enumerable3 = pawn.Map.listerThings.ThingsMatching(scanner.PotentialWorkThingRequest);
									}
									IntVec3 position = pawn.Position;
									IEnumerable<Thing> searchSet = enumerable3;
									Predicate<Thing> validator = predicate;
									thing = GenClosest.ClosestThing_Global(position, searchSet, 99999f, validator, null);
								}
								else
								{
									IntVec3 position = pawn.Position;
									Map map = pawn.Map;
									ThingRequest potentialWorkThingRequest = scanner.PotentialWorkThingRequest;
									PathEndMode pathEndMode = scanner.PathEndMode;
									TraverseParms traverseParams = TraverseParms.For(pawn, scanner.MaxPathDanger(pawn), TraverseMode.ByPawn, false);
									Predicate<Thing> validator = predicate;
									bool forceGlobalSearch = enumerable != null;
									thing = GenClosest.ClosestThingReachable(position, map, potentialWorkThingRequest, pathEndMode, traverseParams, 9999f, validator, enumerable, 0, scanner.LocalRegionsToScanFirst, forceGlobalSearch, RegionType.Set_Passable, false);
								}
								if (thing != null)
								{
									targetInfo = thing;
									workGiver_Scanner = scanner;
								}
							}
							if (scanner.def.scanCells)
							{
								IntVec3 position2 = pawn.Position;
								float num2 = 99999f;
								float num3 = float.MinValue;
								bool prioritized = scanner.Prioritized;
								bool allowUnreachable = scanner.AllowUnreachable;
								Danger maxDanger = scanner.MaxPathDanger(pawn);
								foreach (IntVec3 intVec in scanner.PotentialWorkCellsGlobal(pawn))
								{
									bool flag = false;
									float num4 = (float)(intVec - position2).LengthHorizontalSquared;
									float num5 = 0f;
									if (prioritized)
									{
										if (!intVec.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, intVec, false))
										{
											if (!allowUnreachable && !pawn.CanReach(intVec, scanner.PathEndMode, maxDanger, false, TraverseMode.ByPawn))
											{
												continue;
											}
											num5 = scanner.GetPriority(pawn, intVec);
											if (num5 > num3 || (num5 == num3 && num4 < num2))
											{
												flag = true;
											}
										}
									}
									else if (num4 < num2 && !intVec.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, intVec, false))
									{
										if (!allowUnreachable && !pawn.CanReach(intVec, scanner.PathEndMode, maxDanger, false, TraverseMode.ByPawn))
										{
											continue;
										}
										flag = true;
									}
									if (flag)
									{
										targetInfo = new TargetInfo(intVec, pawn.Map, false);
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
						Log.Error(string.Concat(new object[]
						{
							pawn,
							" threw exception in WorkGiver ",
							workGiver.def.defName,
							": ",
							ex.ToString()
						}), false);
					}
					finally
					{
						Profiler.EndSample();
					}
					if (targetInfo.IsValid)
					{
						Profiler.EndSample();
						pawn.mindState.lastGivenWorkType = workGiver.def.workType;
						Job job3;
						if (targetInfo.HasThing)
						{
							job3 = workGiver_Scanner.JobOnThing(pawn, targetInfo.Thing, false);
						}
						else
						{
							job3 = workGiver_Scanner.JobOnCell(pawn, targetInfo.Cell, false);
						}
						if (job3 != null)
						{
							return new ThinkResult(job3, this, new JobTag?(list[j].def.tagToGive), false);
						}
						Log.ErrorOnce(string.Concat(new object[]
						{
							workGiver_Scanner,
							" provided target ",
							targetInfo,
							" but yielded no actual job for pawn ",
							pawn,
							". The CanGiveJob and JobOnX methods may not be synchronized."
						}), 6112651, false);
					}
					num = workGiver.def.priorityInType;
				}
			}
			Profiler.EndSample();
			return ThinkResult.NoJob;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0003D850 File Offset: 0x0003BC50
		private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
		{
			return (giver.def.canBeDoneByNonColonists || pawn.IsColonist) && (pawn.story == null || !pawn.story.WorkTagIsDisabled(giver.def.workTags)) && !giver.ShouldSkip(pawn, false) && giver.MissingRequiredCapacity(pawn) == null;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0003D8DC File Offset: 0x0003BCDC
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
							Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
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
						if (giver.def.scanCells)
						{
							if (!cell.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, cell, false))
							{
								pawn.mindState.lastGivenWorkType = giver.def.workType;
								return scanner.JobOnCell(pawn, cell, false);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						pawn,
						" threw exception in GiverTryGiveJobTargeted on WorkGiver ",
						giver.def.defName,
						": ",
						ex.ToString()
					}), false);
				}
				result = null;
			}
			return result;
		}

		// Token: 0x040002FB RID: 763
		public bool emergency = false;
	}
}
