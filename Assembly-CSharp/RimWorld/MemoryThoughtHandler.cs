using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052C RID: 1324
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x0600186F RID: 6255 RVA: 0x000D6BDA File Offset: 0x000D4FDA
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001870 RID: 6256 RVA: 0x000D6BF8 File Offset: 0x000D4FF8
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x000D6C14 File Offset: 0x000D5014
		public void ExposeData()
		{
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (this.memories[i].def == null)
					{
						this.memories.RemoveAt(i);
					}
					else
					{
						this.memories[i].pawn = this.pawn;
					}
				}
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x000D6CA8 File Offset: 0x000D50A8
		public void MemoryThoughtInterval()
		{
			Profiler.BeginSample("MemoryThoughtInterval()");
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
			Profiler.EndSample();
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000D6CFC File Offset: 0x000D50FC
		private void RemoveExpiredMemories()
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.ShouldDiscard)
				{
					this.RemoveMemory(thought_Memory);
					if (thought_Memory.def.nextThought != null)
					{
						this.TryGainMemory(thought_Memory.def.nextThought, null);
					}
				}
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000D6D6D File Offset: 0x000D516D
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(def + " is not a memory thought.", false);
			}
			else
			{
				this.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(def), otherPawn);
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000D6DA4 File Offset: 0x000D51A4
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (ThoughtUtility.CanGetThought(this.pawn, newThought.def))
			{
				if (newThought is Thought_MemorySocial && newThought.otherPawn == null && otherPawn == null)
				{
					Log.Error("Can't gain social thought " + newThought.def + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.", false);
				}
				else
				{
					newThought.pawn = this.pawn;
					newThought.otherPawn = otherPawn;
					bool flag;
					if (!newThought.TryMergeWithExistingMemory(out flag))
					{
						this.memories.Add(newThought);
					}
					if (newThought.def.stackLimitForSameOtherPawn >= 0)
					{
						while (this.NumMemoriesInGroup(newThought) > newThought.def.stackLimitForSameOtherPawn)
						{
							this.RemoveMemory(this.OldestMemoryInGroup(newThought));
						}
					}
					if (newThought.def.stackLimit >= 0)
					{
						while (this.NumMemoriesOfDef(newThought.def) > newThought.def.stackLimit)
						{
							this.RemoveMemory(this.OldestMemoryOfDef(newThought.def));
						}
					}
					if (newThought.def.thoughtToMake != null)
					{
						this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn);
					}
					if (flag && newThought.def.showBubble && this.pawn.Spawned)
					{
						MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
					}
				}
			}
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000D6F18 File Offset: 0x000D5318
		public Thought_Memory OldestMemoryInGroup(Thought_Memory group)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.GroupsWith(group))
				{
					if (thought_Memory.age > num)
					{
						result = thought_Memory;
						num = thought_Memory.age;
					}
				}
			}
			return result;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x000D6F8C File Offset: 0x000D538C
		public Thought_Memory OldestMemoryOfDef(ThoughtDef def)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def)
				{
					if (thought_Memory.age > num)
					{
						result = thought_Memory;
						num = thought_Memory.age;
					}
				}
			}
			return result;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000D6FFF File Offset: 0x000D53FF
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.", false);
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x000D7034 File Offset: 0x000D5434
		public int NumMemoriesInGroup(Thought_Memory group)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].GroupsWith(group))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x000D7088 File Offset: 0x000D5488
		public int NumMemoriesOfDef(ThoughtDef def)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x000D70DC File Offset: 0x000D54DC
		public Thought_Memory GetFirstMemoryOfDef(ThoughtDef def)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					return this.memories[i];
				}
			}
			return null;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000D713C File Offset: 0x000D553C
		public void RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef def, Pawn otherPawn)
		{
			for (;;)
			{
				Thought_Memory thought_Memory = this.memories.Find((Thought_Memory x) => x.def == def && x.otherPawn == otherPawn);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x000D7190 File Offset: 0x000D5590
		public void RemoveMemoriesWhereOtherPawnIs(Pawn otherPawn)
		{
			for (;;)
			{
				Thought_Memory thought_Memory = this.memories.Find((Thought_Memory x) => x.otherPawn == otherPawn);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x000D71E0 File Offset: 0x000D55E0
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
			}
			else
			{
				for (;;)
				{
					Thought_Memory thought_Memory = this.memories.Find((Thought_Memory x) => x.def == def);
					if (thought_Memory == null)
					{
						break;
					}
					this.RemoveMemory(thought_Memory);
				}
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000D725C File Offset: 0x000D565C
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
			}
			else
			{
				for (;;)
				{
					Thought_Memory thought_Memory = this.memories.Find((Thought_Memory x) => x.def == def && predicate(x));
					if (thought_Memory == null)
					{
						break;
					}
					this.RemoveMemory(thought_Memory);
				}
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x000D72DC File Offset: 0x000D56DC
		public bool AnyMemoryConcerns(Pawn otherPawn)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x000D732E File Offset: 0x000D572E
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x04000E7F RID: 3711
		public Pawn pawn;

		// Token: 0x04000E80 RID: 3712
		private List<Thought_Memory> memories = new List<Thought_Memory>();
	}
}
