using System;

namespace Verse.AI
{
	// Token: 0x02000ACE RID: 2766
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x06003D5D RID: 15709 RVA: 0x002057A8 File Offset: 0x00203BA8
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x002057C8 File Offset: 0x00203BC8
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x002057E4 File Offset: 0x00203BE4
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x00205800 File Offset: 0x00203C00
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003D61 RID: 15713 RVA: 0x0020581C File Offset: 0x00203C1C
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003D62 RID: 15714 RVA: 0x00205838 File Offset: 0x00203C38
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x0020585C File Offset: 0x00203C5C
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00205884 File Offset: 0x00203C84
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

		// Token: 0x06003D65 RID: 15717 RVA: 0x00205908 File Offset: 0x00203D08
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Job>(seed, this.jobInt);
			seed = Gen.HashCombine<ThinkNode>(seed, this.sourceNodeInt);
			seed = Gen.HashCombine<JobTag?>(seed, this.tag);
			return Gen.HashCombineStruct<bool>(seed, this.fromQueue);
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00205954 File Offset: 0x00203D54
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x00205988 File Offset: 0x00203D88
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

		// Token: 0x06003D68 RID: 15720 RVA: 0x00205A0C File Offset: 0x00203E0C
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00205A2C File Offset: 0x00203E2C
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040026B2 RID: 9906
		private Job jobInt;

		// Token: 0x040026B3 RID: 9907
		private ThinkNode sourceNodeInt;

		// Token: 0x040026B4 RID: 9908
		private JobTag? tag;

		// Token: 0x040026B5 RID: 9909
		private bool fromQueue;
	}
}
