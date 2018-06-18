using System;

namespace RimWorld
{
	// Token: 0x02000534 RID: 1332
	public static class ThoughtMaker
	{
		// Token: 0x060018A8 RID: 6312 RVA: 0x000D8080 File Offset: 0x000D6480
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000D80B4 File Offset: 0x000D64B4
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
