using System;

namespace RimWorld
{
	// Token: 0x02000532 RID: 1330
	public static class ThoughtMaker
	{
		// Token: 0x060018A3 RID: 6307 RVA: 0x000D81DC File Offset: 0x000D65DC
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x000D8210 File Offset: 0x000D6610
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
