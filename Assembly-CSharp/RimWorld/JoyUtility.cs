using System;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000108 RID: 264
	public static class JoyUtility
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x0003BB60 File Offset: 0x00039F60
		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			bool result;
			GameConditionDef gameConditionDef;
			if (map.weatherManager.RainRate >= 0.25f)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				result = false;
			}
			else if (!map.gameConditionManager.AllowEnjoyableOutsideNow(map, out gameConditionDef))
			{
				if (outFailReason != null)
				{
					outFailReason.Append(gameConditionDef.label);
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0003BBE0 File Offset: 0x00039FE0
		public static bool EnjoyableOutsideNow(Pawn pawn, StringBuilder outFailReason = null)
		{
			Map mapHeld = pawn.MapHeld;
			bool result;
			if (mapHeld == null)
			{
				result = true;
			}
			else if (!JoyUtility.EnjoyableOutsideNow(mapHeld, outFailReason))
			{
				result = false;
			}
			else if (!pawn.ComfortableTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("NotEnjoyableOutsideTemperature".Translate());
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0003BC5C File Offset: 0x0003A05C
		public static void JoyTickCheckEnd(Pawn pawn, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f, Building joySource = null)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.", false);
			}
			else
			{
				if (joySource != null)
				{
					if (joySource.def.building.joyKind != null && pawn.CurJob.def.joyKind != joySource.def.building.joyKind)
					{
						Log.ErrorOnce("Joy source joyKind and jobDef.joyKind are not the same. building=" + joySource.ToStringSafe<Building>() + ", jobDef=" + pawn.CurJob.def.ToStringSafe<JobDef>(), joySource.thingIDNumber ^ 876598732, false);
					}
					extraJoyGainFactor *= joySource.GetStatValue(StatDefOf.JoyGainFactor, true);
				}
				pawn.needs.joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f, curJob.def.joyKind);
				if (curJob.def.joySkill != null)
				{
					pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick, false);
				}
				if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				}
				if (pawn.needs.joy.CurLevel > 0.9999f)
				{
					if (fullJoyAction == JoyTickFullJoyAction.EndJob)
					{
						pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					}
					else if (fullJoyAction == JoyTickFullJoyAction.GoToNextToil)
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				}
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0003BE04 File Offset: 0x0003A204
		public static void TryGainRecRoomThought(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
				if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.JoyActivityInImpressiveRecRoom, scoreStageIndex), null);
				}
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0003BE74 File Offset: 0x0003A274
		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0003BEB0 File Offset: 0x0003A2B0
		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
			return !timeAssignmentDef.allowJoy;
		}
	}
}
