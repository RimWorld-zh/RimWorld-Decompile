using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000337 RID: 823
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06000E13 RID: 3603 RVA: 0x00077F80 File Offset: 0x00076380
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
