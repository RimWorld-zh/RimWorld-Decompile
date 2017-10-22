using System.Collections.Generic;

namespace Verse
{
	public struct EdgeSpan
	{
		public IntVec3 root;

		public SpanDirection dir;

		public int length;

		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int i = 0;
				while (true)
				{
					if (i < this.length)
					{
						if (this.dir == SpanDirection.North)
						{
							yield return new IntVec3(this.root.x, 0, this.root.z + i);
							/*Error: Unable to find new state assignment for yield return*/;
						}
						if (this.dir != SpanDirection.East)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return new IntVec3(this.root.x + i, 0, this.root.z);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		public override string ToString()
		{
			return "(root=" + this.root + ", dir=" + this.dir.ToString() + " + length=" + this.length + ")";
		}

		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416L;
			}
			return (ulong)((long)num + 281474976710656L * this.length);
		}
	}
}
