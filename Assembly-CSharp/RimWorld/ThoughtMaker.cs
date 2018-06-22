using System;

namespace RimWorld
{
	// Token: 0x02000530 RID: 1328
	public static class ThoughtMaker
	{
		// Token: 0x0600189F RID: 6303 RVA: 0x000D808C File Offset: 0x000D648C
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x000D80C0 File Offset: 0x000D64C0
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
