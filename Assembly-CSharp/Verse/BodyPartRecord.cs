using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class BodyPartRecord
	{
		public BodyPartDef def = null;

		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		public BodyPartHeight height = BodyPartHeight.Undefined;

		public BodyPartDepth depth = BodyPartDepth.Undefined;

		public float coverage = 1f;

		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		[Unsaved]
		public BodyPartRecord parent = null;

		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		[Unsaved]
		public float coverageAbs = 0f;

		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		public override string ToString()
		{
			return "BodyPartRecord(" + ((this.def == null) ? "NULL_DEF" : this.def.defName) + " parts.Count=" + this.parts.Count + ")";
		}

		public bool IsInGroup(BodyPartGroupDef group)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.groups.Count)
				{
					if (this.groups[num] == group)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public IEnumerable<BodyPartRecord> GetChildParts(string tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				using (IEnumerator<BodyPartRecord> enumerator = this.parts[i].GetChildParts(tag).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						BodyPartRecord record = enumerator.Current;
						yield return record;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0150:
			/*Error near IL_0151: Unexpected return in MoveNext()*/;
		}

		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			int i = 0;
			if (i < this.parts.Count)
			{
				yield return this.parts[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public bool HasChildParts(string tag)
		{
			return this.GetChildParts(tag).Any();
		}

		public IEnumerable<BodyPartRecord> GetConnectedParts(string tag)
		{
			BodyPartRecord ancestor = this;
			while (ancestor.parent != null && ancestor.parent.def.tags.Contains(tag))
			{
				ancestor = ancestor.parent;
			}
			using (IEnumerator<BodyPartRecord> enumerator = ancestor.GetChildParts(tag).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					BodyPartRecord child = enumerator.Current;
					yield return child;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_011c:
			/*Error near IL_011d: Unexpected return in MoveNext()*/;
		}
	}
}
