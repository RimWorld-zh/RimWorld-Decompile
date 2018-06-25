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
		// Token: 0x04000E8E RID: 3726
		public Pawn pawn;

		// Token: 0x04000E8F RID: 3727
		public MemoryThoughtHandler memories;

		// Token: 0x04000E90 RID: 3728
		public SituationalThoughtHandler situational;

		// Token: 0x04000E91 RID: 3729
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04000E92 RID: 3730
		private static List<Thought> tmpTotalMoodOffsetThoughts = new List<Thought>();

		// Token: 0x04000E93 RID: 3731
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		// Token: 0x04000E94 RID: 3732
		private static List<ISocialThought> tmpTotalOpinionOffsetThoughts = new List<ISocialThought>();

		// Token: 0x06001894 RID: 6292 RVA: 0x000D7BD1 File Offset: 0x000D5FD1
		public ThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
			this.memories = new MemoryThoughtHandler(pawn);
			this.situational = new SituationalThoughtHandler(pawn);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000D7BF9 File Offset: 0x000D5FF9
		public void ExposeData()
		{
			Scribe_Deep.Look<MemoryThoughtHandler>(ref this.memories, "memories", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000D7C1B File Offset: 0x000D601B
		public void ThoughtInterval()
		{
			this.situational.SituationalThoughtInterval();
			this.memories.MemoryThoughtInterval();
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000D7C34 File Offset: 0x000D6034
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

		// Token: 0x06001898 RID: 6296 RVA: 0x000D7C98 File Offset: 0x000D6098
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

		// Token: 0x06001899 RID: 6297 RVA: 0x000D7CE4 File Offset: 0x000D60E4
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

		// Token: 0x0600189A RID: 6298 RVA: 0x000D7D9C File Offset: 0x000D619C
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

		// Token: 0x0600189B RID: 6299 RVA: 0x000D7E08 File Offset: 0x000D6208
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

		// Token: 0x0600189C RID: 6300 RVA: 0x000D7E6C File Offset: 0x000D626C
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

		// Token: 0x0600189D RID: 6301 RVA: 0x000D7ED8 File Offset: 0x000D62D8
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

		// Token: 0x0600189E RID: 6302 RVA: 0x000D7F2C File Offset: 0x000D632C
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

		// Token: 0x0600189F RID: 6303 RVA: 0x000D80A8 File Offset: 0x000D64A8
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

		// Token: 0x060018A0 RID: 6304 RVA: 0x000D8120 File Offset: 0x000D6520
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
