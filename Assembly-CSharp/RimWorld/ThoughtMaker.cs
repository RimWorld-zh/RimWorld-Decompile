using System;

namespace RimWorld
{
	// Token: 0x02000532 RID: 1330
	public static class ThoughtMaker
	{
		// Token: 0x060018A2 RID: 6306 RVA: 0x000D8444 File Offset: 0x000D6844
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x000D8478 File Offset: 0x000D6878
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
