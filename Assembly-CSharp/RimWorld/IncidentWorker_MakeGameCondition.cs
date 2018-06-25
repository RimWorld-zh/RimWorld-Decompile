using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_MakeGameCondition : IncidentWorker
	{
		public IncidentWorker_MakeGameCondition()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			bool result;
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", parms.target), 70849667, false);
				result = false;
			}
			else if (gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				result = false;
			}
			else
			{
				List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
				for (int i = 0; i < activeConditions.Count; i++)
				{
					if (!this.def.gameCondition.CanCoexistWith(activeConditions[i].def))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition cond = GameConditionMaker.MakeCondition(this.def.gameCondition, duration, 0);
			gameConditionManager.RegisterCondition(cond);
			base.SendStandardLetter();
			return true;
		}
	}
}
