using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_LayDown
	{
		private const int TicksBetweenSleepZs = 100;

		private const float GroundRestEffectiveness = 0.8f;

		private const int GetUpOrStartJobWhileInBedCheckInterval = 211;

		public static Toil LayDown(TargetIndex bedOrRestSpotIndex, bool hasBed, bool lookForOtherJobs, bool canSleep = true, bool gainRestAndHealth = true)
		{
			Toil layDown = new Toil();
			layDown.initAction = (Action)delegate()
			{
				Pawn actor3 = layDown.actor;
				actor3.pather.StopDead();
				JobDriver curDriver3 = actor3.jobs.curDriver;
				if (hasBed)
				{
					Building_Bed t = (Building_Bed)actor3.CurJob.GetTarget(bedOrRestSpotIndex).Thing;
					if (!t.OccupiedRect().Contains(actor3.Position))
					{
						Log.Error("Can't start LayDown toil because pawn is not in the bed. pawn=" + actor3);
						actor3.jobs.EndCurrentJob(JobCondition.Errored, true);
						return;
					}
					curDriver3.layingDown = LayingDownState.LayingInBed;
				}
				else
				{
					curDriver3.layingDown = LayingDownState.LayingSurface;
				}
				curDriver3.asleep = false;
				if (actor3.mindState.applyBedThoughtsTick == 0)
				{
					actor3.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
					actor3.mindState.applyBedThoughtsOnLeave = false;
				}
				if (actor3.ownership != null && actor3.CurrentBed() != actor3.ownership.OwnedBed)
				{
					ThoughtUtility.RemovePositiveBedroomThoughts(actor3);
				}
			};
			layDown.tickAction = (Action)delegate()
			{
				Pawn actor2 = layDown.actor;
				Job curJob = actor2.CurJob;
				JobDriver curDriver2 = actor2.jobs.curDriver;
				Building_Bed building_Bed = (Building_Bed)curJob.GetTarget(bedOrRestSpotIndex).Thing;
				actor2.GainComfortFromCellIfPossible();
				if (!curDriver2.asleep)
				{
					if (canSleep)
					{
						if (actor2.needs.rest != null && actor2.needs.rest.CurLevel < RestUtility.FallAsleepMaxLevel(actor2))
						{
							goto IL_008f;
						}
						if (curJob.forceSleep)
							goto IL_008f;
					}
				}
				else if (!canSleep)
				{
					curDriver2.asleep = false;
				}
				else if ((actor2.needs.rest == null || actor2.needs.rest.CurLevel >= RestUtility.WakeThreshold(actor2)) && !curJob.forceSleep)
				{
					curDriver2.asleep = false;
				}
				goto IL_00f3;
				IL_00f3:
				if (curDriver2.asleep && gainRestAndHealth && actor2.needs.rest != null)
				{
					float num = (float)((building_Bed == null || !building_Bed.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness)) ? 0.800000011920929 : building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true));
					float num2 = RestUtility.PawnHealthRestEffectivenessFactor(actor2);
					num = (float)(0.699999988079071 * num + 0.30000001192092896 * num * num2);
					actor2.needs.rest.TickResting(num);
				}
				if (actor2.mindState.applyBedThoughtsTick != 0 && actor2.mindState.applyBedThoughtsTick <= Find.TickManager.TicksGame)
				{
					Toils_LayDown.ApplyBedThoughts(actor2);
					actor2.mindState.applyBedThoughtsTick += 60000;
					actor2.mindState.applyBedThoughtsOnLeave = true;
				}
				if (actor2.IsHashIntervalTick(100) && !actor2.Position.Fogged(actor2.Map))
				{
					if (curDriver2.asleep)
					{
						MoteMaker.ThrowMetaIcon(actor2.Position, actor2.Map, ThingDefOf.Mote_SleepZ);
					}
					if (gainRestAndHealth && actor2.health.hediffSet.GetNaturallyHealingInjuredParts().Any())
					{
						MoteMaker.ThrowMetaIcon(actor2.Position, actor2.Map, ThingDefOf.Mote_HealingCross);
					}
				}
				if (actor2.ownership != null && building_Bed != null && !building_Bed.Medical && !building_Bed.owners.Contains(actor2))
				{
					if (actor2.Downed)
					{
						actor2.Position = CellFinder.RandomClosewalkCellNear(actor2.Position, actor2.Map, 1, null);
					}
					actor2.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else if (lookForOtherJobs && actor2.IsHashIntervalTick(211))
				{
					actor2.jobs.CheckForJobOverride();
				}
				return;
				IL_008f:
				curDriver2.asleep = true;
				goto IL_00f3;
			};
			layDown.defaultCompleteMode = ToilCompleteMode.Never;
			if (hasBed)
			{
				layDown.FailOnBedNoLongerUsable(bedOrRestSpotIndex);
			}
			layDown.AddFinishAction((Action)delegate
			{
				Pawn actor = layDown.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				if (actor.mindState.applyBedThoughtsOnLeave)
				{
					Toils_LayDown.ApplyBedThoughts(actor);
				}
				curDriver.layingDown = LayingDownState.NotLaying;
				curDriver.asleep = false;
			});
			return layDown;
		}

		private static void ApplyBedThoughts(Pawn actor)
		{
			if (actor.needs.mood != null)
			{
				Building_Bed building_Bed = actor.CurrentBed();
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBedroom);
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOutside);
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOnGround);
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInCold);
				actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInHeat);
				if (actor.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOutside, null);
				}
				if (building_Bed == null || building_Bed.CostListAdjusted().Count == 0)
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOnGround, null);
				}
				if (actor.AmbientTemperature < actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInCold, null);
				}
				if (actor.AmbientTemperature > actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null))
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInHeat, null);
				}
				if (building_Bed != null && building_Bed == actor.ownership.OwnedBed && !building_Bed.ForPrisoners && !actor.story.traits.HasTrait(TraitDefOf.Ascetic))
				{
					ThoughtDef thoughtDef = null;
					if (building_Bed.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.Bedroom)
					{
						thoughtDef = ThoughtDefOf.SleptInBedroom;
					}
					else if (building_Bed.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.Barracks)
					{
						thoughtDef = ThoughtDefOf.SleptInBarracks;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(building_Bed.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
						}
					}
				}
			}
		}
	}
}
