using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052E RID: 1326
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x04000E83 RID: 3715
		public Pawn pawn;

		// Token: 0x04000E84 RID: 3716
		private List<Thought_Memory> memories = new List<Thought_Memory>();

		// Token: 0x06001872 RID: 6258 RVA: 0x000D6F92 File Offset: 0x000D5392
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x000D6FB0 File Offset: 0x000D53B0
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000D6FCC File Offset: 0x000D53CC
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

		// Token: 0x06001875 RID: 6261 RVA: 0x000D7060 File Offset: 0x000D5460
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

		// Token: 0x06001876 RID: 6262 RVA: 0x000D70B4 File Offset: 0x000D54B4
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

		// Token: 0x06001877 RID: 6263 RVA: 0x000D7125 File Offset: 0x000D5525
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

		// Token: 0x06001878 RID: 6264 RVA: 0x000D715C File Offset: 0x000D555C
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

		// Token: 0x06001879 RID: 6265 RVA: 0x000D72D0 File Offset: 0x000D56D0
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

		// Token: 0x0600187A RID: 6266 RVA: 0x000D7344 File Offset: 0x000D5744
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

		// Token: 0x0600187B RID: 6267 RVA: 0x000D73B7 File Offset: 0x000D57B7
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.", false);
			}
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000D73EC File Offset: 0x000D57EC
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

		// Token: 0x0600187D RID: 6269 RVA: 0x000D7440 File Offset: 0x000D5840
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

		// Token: 0x0600187E RID: 6270 RVA: 0x000D7494 File Offset: 0x000D5894
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

		// Token: 0x0600187F RID: 6271 RVA: 0x000D74F4 File Offset: 0x000D58F4
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

		// Token: 0x06001880 RID: 6272 RVA: 0x000D7548 File Offset: 0x000D5948
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

		// Token: 0x06001881 RID: 6273 RVA: 0x000D7598 File Offset: 0x000D5998
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

		// Token: 0x06001882 RID: 6274 RVA: 0x000D7614 File Offset: 0x000D5A14
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

		// Token: 0x06001883 RID: 6275 RVA: 0x000D7694 File Offset: 0x000D5A94
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

		// Token: 0x06001884 RID: 6276 RVA: 0x000D76E6 File Offset: 0x000D5AE6
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}
	}
}
