using System;

namespace Verse.AI
{
	// Token: 0x02000ACA RID: 2762
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x06003D5A RID: 15706 RVA: 0x00205BA0 File Offset: 0x00203FA0
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003D5B RID: 15707 RVA: 0x00205BC0 File Offset: 0x00203FC0
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003D5C RID: 15708 RVA: 0x00205BDC File Offset: 0x00203FDC
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003D5D RID: 15709 RVA: 0x00205BF8 File Offset: 0x00203FF8
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x00205C14 File Offset: 0x00204014
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x00205C30 File Offset: 0x00204030
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x00205C54 File Offset: 0x00204054
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00205C7C File Offset: 0x0020407C
		public override string ToString()
		{
			string text = (this.Job == null) ? "null" : this.Job.ToString();
			string text2 = (this.SourceNode == null) ? "null" : this.SourceNode.ToString();
			return string.Concat(new string[]
			{
				"(job=",
				text,
				" sourceNode=",
				text2,
				")"
			});
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00205D00 File Offset: 0x00204100
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Job>(seed, this.jobInt);
			seed = Gen.HashCombine<ThinkNode>(seed, this.sourceNodeInt);
			seed = Gen.HashCombine<JobTag?>(seed, this.tag);
			return Gen.HashCombineStruct<bool>(seed, this.fromQueue);
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00205D4C File Offset: 0x0020414C
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00205D80 File Offset: 0x00204180
		public bool Equals(ThinkResult other)
		{
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? jobTag = this.tag;
				JobTag valueOrDefault = jobTag.GetValueOrDefault();
				JobTag? jobTag2 = other.tag;
				if (valueOrDefault == jobTag2.GetValueOrDefault() && jobTag != null == (jobTag2 != null))
				{
					return this.fromQueue == other.fromQueue;
				}
			}
			return false;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00205E04 File Offset: 0x00204204
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00205E24 File Offset: 0x00204224
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040026AD RID: 9901
		private Job jobInt;

		// Token: 0x040026AE RID: 9902
		private ThinkNode sourceNodeInt;

		// Token: 0x040026AF RID: 9903
		private JobTag? tag;

		// Token: 0x040026B0 RID: 9904
		private bool fromQueue;
	}
}
