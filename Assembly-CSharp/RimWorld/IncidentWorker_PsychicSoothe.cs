using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033A RID: 826
	public class IncidentWorker_PsychicSoothe : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06000E18 RID: 3608 RVA: 0x0007815C File Offset: 0x0007655C
		protected override void DoConditionAndLetter(Map map, int duration, Gender gender)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration, 0);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			string text = "LetterIncidentPsychicSoothe".Translate(new object[]
			{
				gender.ToString().Translate().ToLower()
			});
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicSoothe".Translate(), text, LetterDefOf.PositiveEvent, null);
		}
	}
}
