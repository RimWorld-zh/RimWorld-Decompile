using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x02000531 RID: 1329
	public sealed class ThoughtHandler : IExposable
	{
		// Token: 0x04000E92 RID: 3730
		public Pawn pawn;

		// Token: 0x04000E93 RID: 3731
		public MemoryThoughtHandler memories;

		// Token: 0x04000E94 RID: 3732
		public SituationalThoughtHandler situational;

		// Token: 0x04000E95 RID: 3733
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04000E96 RID: 3734
		private static List<Thought> tmpTotalMoodOffsetThoughts = new List<Thought>();

		// Token: 0x04000E97 RID: 3735
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		// Token: 0x04000E98 RID: 3736
		private static List<ISocialThought> tmpTotalOpinionOffsetThoughts = new List<ISocialThought>();

		// Token: 0x06001893 RID: 6291 RVA: 0x000D7E39 File Offset: 0x000D6239
		public ThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
			this.memories = new MemoryThoughtHandler(pawn);
			this.situational = new SituationalThoughtHandler(pawn);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x000D7E61 File Offset: 0x000D6261
		public void ExposeData()
		{
			Scribe_Deep.Look<MemoryThoughtHandler>(ref this.memories, "memories", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000D7E83 File Offset: 0x000D6283
		public void ThoughtInterval()
		{
			this.situational.SituationalThoughtInterval();
			this.memories.MemoryThoughtInterval();
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000D7E9C File Offset: 0x000D629C
		public void GetAllMoodThoughts(List<Thought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				Thought_Memory thought_Memory = list[i];
				if (thought_Memory.MoodOffset() != 0f)
				{
					outThoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(outThoughts);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000D7F00 File Offset: 0x000D6300
		public void GetMoodThoughts(Thought group, List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!outThoughts[i].GroupsWith(group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x000D7F4C File Offset: 0x000D634C
		public float MoodOffsetOfGroup(Thought group)
		{
			this.GetMoodThoughts(group, ThoughtHandler.tmpThoughts);
			float result;
			if (!ThoughtHandler.tmpThoughts.Any<Thought>())
			{
				result = 0f;
			}
			else
			{
				float num = 0f;
				float num2 = 1f;
				float num3 = 0f;
				for (int i = 0; i < ThoughtHandler.tmpThoughts.Count; i++)
				{
					Thought thought = ThoughtHandler.tmpThoughts[i];
					num += thought.MoodOffset();
					num3 += num2;
					num2 *= thought.def.stackedEffectMultiplier;
				}
				float num4 = num / (float)ThoughtHandler.tmpThoughts.Count;
				ThoughtHandler.tmpThoughts.Clear();
				result = num4 * num3;
			}
			return result;
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x000D8004 File Offset: 0x000D6404
		public void GetDistinctMoodThoughtGroups(List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				Thought other = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (outThoughts[j].GroupsWith(other))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000D8070 File Offset: 0x000D6470
		public float TotalMoodOffset()
		{
			this.GetDistinctMoodThoughtGroups(ThoughtHandler.tmpTotalMoodOffsetThoughts);
			float num = 0f;
			for (int i = 0; i < ThoughtHandler.tmpTotalMoodOffsetThoughts.Count; i++)
			{
				num += this.MoodOffsetOfGroup(ThoughtHandler.tmpTotalMoodOffsetThoughts[i]);
			}
			ThoughtHandler.tmpTotalMoodOffsetThoughts.Clear();
			return num;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000D80D4 File Offset: 0x000D64D4
		public void GetSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				ISocialThought socialThought = list[i] as ISocialThought;
				if (socialThought != null && socialThought.OtherPawn() == otherPawn)
				{
					outThoughts.Add(socialThought);
				}
			}
			this.situational.AppendSocialThoughts(otherPawn, outThoughts);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000D8140 File Offset: 0x000D6540
		public void GetSocialThoughts(Pawn otherPawn, ISocialThought group, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!((Thought)outThoughts[i]).GroupsWith((Thought)group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000D8194 File Offset: 0x000D6594
		public int OpinionOffsetOfGroup(ISocialThought group, Pawn otherPawn)
		{
			this.GetSocialThoughts(otherPawn, group, ThoughtHandler.tmpSocialThoughts);
			for (int i = ThoughtHandler.tmpSocialThoughts.Count - 1; i >= 0; i--)
			{
				if (ThoughtHandler.tmpSocialThoughts[i].OpinionOffset() == 0f)
				{
					ThoughtHandler.tmpSocialThoughts.RemoveAt(i);
				}
			}
			int result;
			if (!ThoughtHandler.tmpSocialThoughts.Any<ISocialThought>())
			{
				Profiler.EndSample();
				result = 0;
			}
			else
			{
				ThoughtDef def = ((Thought)group).def;
				if (def.IsMemory && def.stackedEffectMultiplier != 1f)
				{
					ThoughtHandler.tmpSocialThoughts.Sort((ISocialThought a, ISocialThought b) => ((Thought_Memory)a).age.CompareTo(((Thought_Memory)b).age));
				}
				float num = 0f;
				float num2 = 1f;
				for (int j = 0; j < ThoughtHandler.tmpSocialThoughts.Count; j++)
				{
					num += ThoughtHandler.tmpSocialThoughts[j].OpinionOffset() * num2;
					num2 *= ((Thought)ThoughtHandler.tmpSocialThoughts[j]).def.stackedEffectMultiplier;
				}
				ThoughtHandler.tmpSocialThoughts.Clear();
				if (num == 0f)
				{
					result = 0;
				}
				else if (num > 0f)
				{
					result = Mathf.Max(Mathf.RoundToInt(num), 1);
				}
				else
				{
					result = Mathf.Min(Mathf.RoundToInt(num), -1);
				}
			}
			return result;
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000D8310 File Offset: 0x000D6710
		public void GetDistinctSocialThoughtGroups(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				ISocialThought socialThought = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (((Thought)outThoughts[j]).GroupsWith((Thought)socialThought))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000D8388 File Offset: 0x000D6788
		public int TotalOpinionOffset(Pawn otherPawn)
		{
			this.GetDistinctSocialThoughtGroups(otherPawn, ThoughtHandler.tmpTotalOpinionOffsetThoughts);
			int num = 0;
			for (int i = 0; i < ThoughtHandler.tmpTotalOpinionOffsetThoughts.Count; i++)
			{
				num += this.OpinionOffsetOfGroup(ThoughtHandler.tmpTotalOpinionOffsetThoughts[i], otherPawn);
			}
			ThoughtHandler.tmpTotalOpinionOffsetThoughts.Clear();
			return num;
		}
	}
}
