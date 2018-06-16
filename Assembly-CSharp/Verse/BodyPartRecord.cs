using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AFD RID: 2813
	public class BodyPartRecord
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x0020CB38 File Offset: 0x0020AF38
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0020CB58 File Offset: 0x0020AF58
		public string Label
		{
			get
			{
				return (!this.customLabel.NullOrEmpty()) ? this.customLabel : this.def.label;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0020CB94 File Offset: 0x0020AF94
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0020CBB4 File Offset: 0x0020AFB4
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0020CBD4 File Offset: 0x0020AFD4
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0020CBF4 File Offset: 0x0020AFF4
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x0020CC18 File Offset: 0x0020B018
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"BodyPartRecord(",
				(this.def == null) ? "NULL_DEF" : this.def.defName,
				" parts.Count=",
				this.parts.Count,
				")"
			});
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0020CC88 File Offset: 0x0020B088
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

		// Token: 0x06003E4E RID: 15950 RVA: 0x0020CCD8 File Offset: 0x0020B0D8
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				foreach (BodyPartRecord record in this.parts[i].GetChildParts(tag))
				{
					yield return record;
				}
			}
			yield break;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0020CD0C File Offset: 0x0020B10C
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				yield return this.parts[i];
			}
			yield break;
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0020CD38 File Offset: 0x0020B138
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x0020CD5C File Offset: 0x0020B15C
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord ancestor = this;
			while (ancestor.parent != null && ancestor.parent.def.tags.Contains(tag))
			{
				ancestor = ancestor.parent;
			}
			foreach (BodyPartRecord child in ancestor.GetChildParts(tag))
			{
				yield return child;
			}
			yield break;
		}

		// Token: 0x0400275F RID: 10079
		public BodyDef body;

		// Token: 0x04002760 RID: 10080
		public BodyPartDef def = null;

		// Token: 0x04002761 RID: 10081
		[MustTranslate]
		public string customLabel;

		// Token: 0x04002762 RID: 10082
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04002763 RID: 10083
		public BodyPartHeight height = BodyPartHeight.Undefined;

		// Token: 0x04002764 RID: 10084
		public BodyPartDepth depth = BodyPartDepth.Undefined;

		// Token: 0x04002765 RID: 10085
		public float coverage = 1f;

		// Token: 0x04002766 RID: 10086
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x04002767 RID: 10087
		[Unsaved]
		public BodyPartRecord parent = null;

		// Token: 0x04002768 RID: 10088
		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		// Token: 0x04002769 RID: 10089
		[Unsaved]
		public float coverageAbs = 0f;
	}
}
