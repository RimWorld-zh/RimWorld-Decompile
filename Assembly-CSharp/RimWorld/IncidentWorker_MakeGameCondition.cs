using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_MakeGameCondition : IncidentWorker
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			GameConditionManager gameConditionManager = target.GameConditionManager;
			bool result;
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", target), 70849667);
				result = false;
			}
			else if (gameConditionManager.ConditionIsActive(base.def.gameCondition))
			{
				result = false;
			}
			else
			{
				List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
				for (int i = 0; i < activeConditions.Count; i++)
				{
					if (!base.def.gameCondition.CanCoexistWith(activeConditions[i].def))
						goto IL_0078;
				}
				result = true;
			}
			goto IL_0097;
			IL_0097:
			return result;
			IL_0078:
			result = false;
			goto IL_0097;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			int duration = Mathf.RoundToInt((float)(base.def.durationDays.RandomInRange * 60000.0));
			GameCondition cond = GameConditionMaker.MakeCondition(base.def.gameCondition, duration, 0);
			gameConditionManager.RegisterCondition(cond);
			base.SendStandardLetter();
			return true;
		}
	}
}
