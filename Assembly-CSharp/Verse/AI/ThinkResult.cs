using System;

namespace Verse.AI
{
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		private Job jobInt;

		private ThinkNode sourceNodeInt;

		private JobTag? tag;

		private bool fromQueue;

		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

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

		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
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
				return new ThinkResult(null, null, null, false);
			}
		}

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

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Job>(seed, this.jobInt);
			seed = Gen.HashCombine<ThinkNode>(seed, this.sourceNodeInt);
			seed = Gen.HashCombine<JobTag?>(seed, this.tag);
			return Gen.HashCombineStruct<bool>(seed, this.fromQueue);
		}

		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

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
