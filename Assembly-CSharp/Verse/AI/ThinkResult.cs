using System;

namespace Verse.AI
{
	// Token: 0x02000ACE RID: 2766
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x06003D5F RID: 15711 RVA: 0x0020587C File Offset: 0x00203C7C
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x0020589C File Offset: 0x00203C9C
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003D61 RID: 15713 RVA: 0x002058B8 File Offset: 0x00203CB8
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003D62 RID: 15714 RVA: 0x002058D4 File Offset: 0x00203CD4
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x002058F0 File Offset: 0x00203CF0
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003D64 RID: 15716 RVA: 0x0020590C File Offset: 0x00203D0C
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x00205930 File Offset: 0x00203D30
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00205958 File Offset: 0x00203D58
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

		// Token: 0x06003D67 RID: 15719 RVA: 0x002059DC File Offset: 0x00203DDC
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Job>(seed, this.jobInt);
			seed = Gen.HashCombine<ThinkNode>(seed, this.sourceNodeInt);
			seed = Gen.HashCombine<JobTag?>(seed, this.tag);
			return Gen.HashCombineStruct<bool>(seed, this.fromQueue);
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00205A28 File Offset: 0x00203E28
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00205A5C File Offset: 0x00203E5C
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

		// Token: 0x06003D6A RID: 15722 RVA: 0x00205AE0 File Offset: 0x00203EE0
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x00205B00 File Offset: 0x00203F00
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
