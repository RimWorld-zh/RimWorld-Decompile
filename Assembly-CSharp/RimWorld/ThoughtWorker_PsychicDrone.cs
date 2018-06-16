using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000228 RID: 552
	public class ThoughtWorker_PsychicDrone : ThoughtWorker
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x00059828 File Offset: 0x00057C28
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			CompPsychicDrone compPsychicDrone = ThoughtWorker_PsychicDrone.PsychicDroneEmanator(p.Map);
			if (compPsychicDrone != null)
			{
				psychicDroneLevel = compPsychicDrone.DroneLevel;
			}
			GameCondition_PsychicEmanation activeCondition = p.Map.gameConditionManager.GetActiveCondition<GameCondition_PsychicEmanation>();
			if (activeCondition != null && activeCondition.gender == p.gender && activeCondition.def.droneLevel > psychicDroneLevel)
			{
				psychicDroneLevel = activeCondition.def.droneLevel;
			}
			ThoughtState result;
			switch (psychicDroneLevel)
			{
			case PsychicDroneLevel.None:
				result = false;
				break;
			case PsychicDroneLevel.GoodMedium:
				result = ThoughtState.ActiveAtStage(0);
				break;
			case PsychicDroneLevel.BadLow:
				result = ThoughtState.ActiveAtStage(1);
				break;
			case PsychicDroneLevel.BadMedium:
				result = ThoughtState.ActiveAtStage(2);
				break;
			case PsychicDroneLevel.BadHigh:
				result = ThoughtState.ActiveAtStage(3);
				break;
			case PsychicDroneLevel.BadExtreme:
				result = ThoughtState.ActiveAtStage(4);
				break;
			default:
				throw new NotImplementedException();
			}
			return result;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0005990C File Offset: 0x00057D0C
		private static CompPsychicDrone PsychicDroneEmanator(Map map)
		{
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.PsychicDroneEmanator);
			CompPsychicDrone result;
			if (!list.Any<Thing>())
			{
				result = null;
			}
			else
			{
				result = list[0].TryGetComp<CompPsychicDrone>();
			}
			return result;
		}
	}
}
