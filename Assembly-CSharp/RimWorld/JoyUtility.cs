using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class JoyUtility
	{
		public const float BaseJoyGainPerHour = 0.36f;

		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			bool result;
			GameConditionDef gameConditionDef = default(GameConditionDef);
			if (map.weatherManager.RainRate >= 0.25)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				result = false;
			}
			else if (!map.gameConditionManager.AllowEnjoyableOutsideNow(out gameConditionDef))
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

		public static void JoyTickCheckEnd(Pawn pawn, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.");
			}
			else
			{
				pawn.needs.joy.GainJoy((float)(extraJoyGainFactor * curJob.def.joyGainRate * 0.00014400000509340316), curJob.def.joyKind);
				if (curJob.def.joySkill != null)
				{
					pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick, false);
				}
				if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				}
				if (pawn.needs.joy.CurLevel > 0.99989998340606689)
				{
					switch (fullJoyAction)
					{
					case JoyTickFullJoyAction.EndJob:
					{
						pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
						break;
					}
					case JoyTickFullJoyAction.GoToNextToil:
					{
						pawn.jobs.curDriver.ReadyForNextToil();
						break;
					}
					}
				}
			}
		}

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

		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return (byte)((lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds) ? 1 : 0) != 0;
		}

		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
			return (byte)((!timeAssignmentDef.allowJoy) ? 1 : 0) != 0;
		}
	}
}
