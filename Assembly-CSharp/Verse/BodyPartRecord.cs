using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class BodyPartRecord
	{
		public BodyPartDef def;

		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		public BodyPartHeight height;

		public BodyPartDepth depth;

		public float coverage = 1f;

		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		[Unsaved]
		public BodyPartRecord parent;

		[Unsaved]
		public float coverageAbsWithChildren;

		[Unsaved]
		public float coverageAbs;

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
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
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
			IL_0148:
			/*Error near IL_0149: Unexpected return in MoveNext()*/;
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
			IL_0116:
			/*Error near IL_0117: Unexpected return in MoveNext()*/;
		}
	}
}
