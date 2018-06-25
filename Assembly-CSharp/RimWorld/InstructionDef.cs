using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EF RID: 751
	public class InstructionDef : Def
	{
		// Token: 0x04000803 RID: 2051
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x04000804 RID: 2052
		[MustTranslate]
		public string text;

		// Token: 0x04000805 RID: 2053
		public bool startCentered = false;

		// Token: 0x04000806 RID: 2054
		public bool tutorialModeOnly = true;

		// Token: 0x04000807 RID: 2055
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x04000808 RID: 2056
		public InstructionDef eventTagInitiateSource;

		// Token: 0x04000809 RID: 2057
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x0400080A RID: 2058
		[NoTranslate]
		public List<string> actionTagsAllowed = null;

		// Token: 0x0400080B RID: 2059
		[MustTranslate]
		public string rejectInputMessage = null;

		// Token: 0x0400080C RID: 2060
		public ConceptDef concept = null;

		// Token: 0x0400080D RID: 2061
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x0400080E RID: 2062
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x0400080F RID: 2063
		public int targetCount;

		// Token: 0x04000810 RID: 2064
		public ThingDef thingDef;

		// Token: 0x04000811 RID: 2065
		public RecipeDef recipeDef;

		// Token: 0x04000812 RID: 2066
		public int recipeTargetCount = 1;

		// Token: 0x04000813 RID: 2067
		public ThingDef giveOnActivateDef;

		// Token: 0x04000814 RID: 2068
		public int giveOnActivateCount;

		// Token: 0x04000815 RID: 2069
		public bool endTutorial = false;

		// Token: 0x04000816 RID: 2070
		public bool resetBuildDesignatorStuffs = false;

		// Token: 0x04000817 RID: 2071
		private static List<string> tmpParseErrors = new List<string>();

		// Token: 0x06000C64 RID: 3172 RVA: 0x0006DF8C File Offset: 0x0006C38C
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
	}
}
