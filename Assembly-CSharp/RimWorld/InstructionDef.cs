using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002ED RID: 749
	public class InstructionDef : Def
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0006DD80 File Offset: 0x0006C180
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
			}
			if (this.eventTagInitiate.NullOrEmpty())
			{
				yield return "no eventTagInitiate";
			}
			InstructionDef.tmpParseErrors.Clear();
			this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
			for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i++)
			{
				yield return "text error: " + InstructionDef.tmpParseErrors[i];
			}
			yield break;
		}

		// Token: 0x040007FE RID: 2046
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x040007FF RID: 2047
		[MustTranslate]
		public string text;

		// Token: 0x04000800 RID: 2048
		public bool startCentered = false;

		// Token: 0x04000801 RID: 2049
		public bool tutorialModeOnly = true;

		// Token: 0x04000802 RID: 2050
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x04000803 RID: 2051
		public InstructionDef eventTagInitiateSource;

		// Token: 0x04000804 RID: 2052
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x04000805 RID: 2053
		[NoTranslate]
		public List<string> actionTagsAllowed = null;

		// Token: 0x04000806 RID: 2054
		[MustTranslate]
		public string rejectInputMessage = null;

		// Token: 0x04000807 RID: 2055
		public ConceptDef concept = null;

		// Token: 0x04000808 RID: 2056
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x04000809 RID: 2057
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x0400080A RID: 2058
		public int targetCount;

		// Token: 0x0400080B RID: 2059
		public ThingDef thingDef;

		// Token: 0x0400080C RID: 2060
		public RecipeDef recipeDef;

		// Token: 0x0400080D RID: 2061
		public int recipeTargetCount = 1;

		// Token: 0x0400080E RID: 2062
		public ThingDef giveOnActivateDef;

		// Token: 0x0400080F RID: 2063
		public int giveOnActivateCount;

		// Token: 0x04000810 RID: 2064
		public bool endTutorial = false;

		// Token: 0x04000811 RID: 2065
		public bool resetBuildDesignatorStuffs = false;

		// Token: 0x04000812 RID: 2066
		private static List<string> tmpParseErrors = new List<string>();
	}
}
