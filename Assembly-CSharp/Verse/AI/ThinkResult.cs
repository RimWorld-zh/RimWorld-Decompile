using System;

namespace Verse.AI
{
	// Token: 0x02000ACD RID: 2765
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x040026B5 RID: 9909
		private Job jobInt;

		// Token: 0x040026B6 RID: 9910
		private ThinkNode sourceNodeInt;

		// Token: 0x040026B7 RID: 9911
		private JobTag? tag;

		// Token: 0x040026B8 RID: 9912
		private bool fromQueue;

		// Token: 0x06003D5E RID: 15710 RVA: 0x00205FAC File Offset: 0x002043AC
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x00205FCC File Offset: 0x002043CC
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x00205FE8 File Offset: 0x002043E8
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003D61 RID: 15713 RVA: 0x00206004 File Offset: 0x00204404
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003D62 RID: 15714 RVA: 0x00206020 File Offset: 0x00204420
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x0020603C File Offset: 0x0020443C
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003D64 RID: 15716 RVA: 0x00206060 File Offset: 0x00204460
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00206088 File Offset: 0x00204488
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

		// Token: 0x06003D66 RID: 15718 RVA: 0x0020610C File Offset: 0x0020450C
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Job>(seed, this.jobInt);
			seed = Gen.HashCombine<ThinkNode>(seed, this.sourceNodeInt);
			seed = Gen.HashCombine<JobTag?>(seed, this.tag);
			return Gen.HashCombineStruct<bool>(seed, this.fromQueue);
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x00206158 File Offset: 0x00204558
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0020618C File Offset: 0x0020458C
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

		// Token: 0x06003D69 RID: 15721 RVA: 0x00206210 File Offset: 0x00204610
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00206230 File Offset: 0x00204630
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}
	}
}
