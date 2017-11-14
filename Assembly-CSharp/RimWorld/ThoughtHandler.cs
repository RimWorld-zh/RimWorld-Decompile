using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public sealed class ThoughtHandler : IExposable
	{
		public Pawn pawn;

		public MemoryThoughtHandler memories;

		public SituationalThoughtHandler situational;

		private static List<Thought> tmpThoughts = new List<Thought>();

		private static List<Thought> tmpTotalMoodOffsetThoughts = new List<Thought>();

		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		private static List<ISocialThought> tmpTotalOpinionOffsetThoughts = new List<ISocialThought>();

		public ThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
			this.memories = new MemoryThoughtHandler(pawn);
			this.situational = new SituationalThoughtHandler(pawn);
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<MemoryThoughtHandler>(ref this.memories, "memories", new object[1]
			{
				this.pawn
			});
		}

		public void ThoughtInterval()
		{
			this.situational.SituationalThoughtInterval();
			this.memories.MemoryThoughtInterval();
		}

		public void GetAllMoodThoughts(List<Thought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				Thought_Memory thought_Memory = list[i];
				if (thought_Memory.MoodOffset() != 0.0)
				{
					outThoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(outThoughts);
		}

		public void GetMoodThoughts(Thought group, List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int num = outThoughts.Count - 1; num >= 0; num--)
			{
				if (!outThoughts[num].GroupsWith(group))
				{
					outThoughts.RemoveAt(num);
				}
			}
		}

		public float MoodOffsetOfGroup(Thought group)
		{
			this.GetMoodThoughts(group, ThoughtHandler.tmpThoughts);
			if (!ThoughtHandler.tmpThoughts.Any())
			{
				return 0f;
			}
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
			return num4 * num3;
		}

		public void GetDistinctMoodThoughtGroups(List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int num = outThoughts.Count - 1; num >= 0; num--)
			{
				Thought other = outThoughts[num];
				int num2 = 0;
				while (num2 < num)
				{
					if (!outThoughts[num2].GroupsWith(other))
					{
						num2++;
						continue;
					}
					outThoughts.RemoveAt(num);
					break;
				}
			}
		}

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

		public void GetSocialThoughts(Pawn otherPawn, ISocialThought group, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int num = outThoughts.Count - 1; num >= 0; num--)
			{
				if (!((Thought)outThoughts[num]).GroupsWith((Thought)group))
				{
					outThoughts.RemoveAt(num);
				}
			}
		}

		public int OpinionOffsetOfGroup(ISocialThought group, Pawn otherPawn)
		{
			this.GetSocialThoughts(otherPawn, group, ThoughtHandler.tmpSocialThoughts);
			for (int num = ThoughtHandler.tmpSocialThoughts.Count - 1; num >= 0; num--)
			{
				if (ThoughtHandler.tmpSocialThoughts[num].OpinionOffset() == 0.0)
				{
					ThoughtHandler.tmpSocialThoughts.RemoveAt(num);
				}
			}
			if (!ThoughtHandler.tmpSocialThoughts.Any())
			{
				return 0;
			}
			ThoughtDef def = ((Thought)group).def;
			if (def.IsMemory && def.stackedEffectMultiplier != 1.0)
			{
				ThoughtHandler.tmpSocialThoughts.Sort((ISocialThought a, ISocialThought b) => ((Thought_Memory)a).age.CompareTo(((Thought_Memory)b).age));
			}
			float num2 = 0f;
			float num3 = 1f;
			for (int i = 0; i < ThoughtHandler.tmpSocialThoughts.Count; i++)
			{
				num2 += ThoughtHandler.tmpSocialThoughts[i].OpinionOffset() * num3;
				num3 *= ((Thought)ThoughtHandler.tmpSocialThoughts[i]).def.stackedEffectMultiplier;
			}
			ThoughtHandler.tmpSocialThoughts.Clear();
			if (num2 == 0.0)
			{
				return 0;
			}
			if (num2 > 0.0)
			{
				return Mathf.Max(Mathf.RoundToInt(num2), 1);
			}
			return Mathf.Min(Mathf.RoundToInt(num2), -1);
		}

		public void GetDistinctSocialThoughtGroups(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int num = outThoughts.Count - 1; num >= 0; num--)
			{
				ISocialThought socialThought = outThoughts[num];
				int num2 = 0;
				while (num2 < num)
				{
					if (!((Thought)outThoughts[num2]).GroupsWith((Thought)socialThought))
					{
						num2++;
						continue;
					}
					outThoughts.RemoveAt(num);
					break;
				}
			}
		}

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
