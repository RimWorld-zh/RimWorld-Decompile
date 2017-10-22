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
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				foreach (BodyPartRecord childPart in this.parts[i].GetChildParts(tag))
				{
					yield return childPart;
				}
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
			foreach (BodyPartRecord childPart in ancestor.GetChildParts(tag))
			{
				yield return childPart;
			}
		}
	}
}
