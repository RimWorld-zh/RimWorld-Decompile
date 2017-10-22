using System;

namespace Verse.AI
{
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		private Job jobInt;

		private ThinkNode sourceNodeInt;

		private JobTag? tag;

		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, default(JobTag?));
			}
		}

		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = default(JobTag?))
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
		}

		public override string ToString()
		{
			string text = (this.Job == null) ? "null" : this.Job.ToString();
			string text2 = (this.SourceNode == null) ? "null" : this.SourceNode.ToString();
			return "(job=" + text + " sourceNode=" + text2 + ")";
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine(seed, this.jobInt);
			seed = Gen.HashCombine(seed, this.sourceNodeInt);
			return Gen.HashCombine(seed, this.tag);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ThinkResult))
			{
				return false;
			}
			return this.Equals((ThinkResult)obj);
		}

		public bool Equals(ThinkResult other)
		{
			int result;
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? nullable = this.tag;
				JobTag valueOrDefault = nullable.GetValueOrDefault();
				JobTag? nullable2 = other.tag;
				result = ((valueOrDefault == nullable2.GetValueOrDefault() && nullable.HasValue == nullable2.HasValue) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}
	}
}
