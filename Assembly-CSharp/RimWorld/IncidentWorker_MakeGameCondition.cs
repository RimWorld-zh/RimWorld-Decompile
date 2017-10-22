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
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", target), 70849667);
				return false;
			}
			if (gameConditionManager.ConditionIsActive(base.def.gameCondition))
			{
				return false;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!base.def.gameCondition.CanCoexistWith(activeConditions[i].def))
				{
					return false;
				}
			}
			return true;
		}

		public override bool TryExecute(IncidentParms parms)
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
