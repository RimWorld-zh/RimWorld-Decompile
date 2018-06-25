using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000339 RID: 825
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06000E16 RID: 3606 RVA: 0x000780D8 File Offset: 0x000764D8
		protected override void DoConditionAndLetter(Map map, int duration, Gender gender)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, duration, 0);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			string text = "LetterIncidentPsychicDrone".Translate(new object[]
			{
				gender.ToString().Translate().ToLower()
			});
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDrone".Translate(), text, LetterDefOf.NegativeEvent, null);
		}
	}
}
