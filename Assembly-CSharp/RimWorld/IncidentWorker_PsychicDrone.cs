using System;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		public IncidentWorker_PsychicDrone()
		{
		}

		protected override void DoConditionAndLetter(Map map, int duration, Gender gender)
		{
			PsychicDroneLevel psychicDroneLevel = ExpectationsUtility.CurrentExpectationFor(map).psychicDroneLevel;
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, duration, 0);
			gameCondition_PsychicEmanation.gender = gender;
			gameCondition_PsychicEmanation.level = psychicDroneLevel;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			string label = "LetterLabelPsychicDrone".Translate() + " (" + psychicDroneLevel.GetLabel() + ")";
			string text = "LetterIncidentPsychicDrone".Translate(new object[]
			{
				gender.ToString().Translate().ToLower(),
				psychicDroneLevel.GetLabel()
			});
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, null);
		}
	}
}
