using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AFB RID: 2811
	public class BodyPartRecord
	{
		// Token: 0x0400275B RID: 10075
		public BodyDef body;

		// Token: 0x0400275C RID: 10076
		[TranslationHandle]
		public BodyPartDef def = null;

		// Token: 0x0400275D RID: 10077
		[MustTranslate]
		public string customLabel;

		// Token: 0x0400275E RID: 10078
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;

		// Token: 0x0400275F RID: 10079
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04002760 RID: 10080
		public BodyPartHeight height = BodyPartHeight.Undefined;

		// Token: 0x04002761 RID: 10081
		public BodyPartDepth depth = BodyPartDepth.Undefined;

		// Token: 0x04002762 RID: 10082
		public float coverage = 1f;

		// Token: 0x04002763 RID: 10083
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x04002764 RID: 10084
		[Unsaved]
		public BodyPartRecord parent = null;

		// Token: 0x04002765 RID: 10085
		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		// Token: 0x04002766 RID: 10086
		[Unsaved]
		public float coverageAbs = 0f;

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0020D064 File Offset: 0x0020B464
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0020D084 File Offset: 0x0020B484
		public string Label
		{
			get
			{
				return (!this.customLabel.NullOrEmpty()) ? this.customLabel : this.def.label;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0020D0C0 File Offset: 0x0020B4C0
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0020D0E0 File Offset: 0x0020B4E0
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0020D100 File Offset: 0x0020B500
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0020D120 File Offset: 0x0020B520
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0020D144 File Offset: 0x0020B544
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

		// Token: 0x06003E4E RID: 15950 RVA: 0x0020D1B2 File Offset: 0x0020B5B2
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0020D1C4 File Offset: 0x0020B5C4
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

		// Token: 0x06003E50 RID: 15952 RVA: 0x0020D214 File Offset: 0x0020B614
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

		// Token: 0x06003E51 RID: 15953 RVA: 0x0020D248 File Offset: 0x0020B648
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				yield return this.parts[i];
			}
			yield break;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0020D274 File Offset: 0x0020B674
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0020D298 File Offset: 0x0020B698
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
	}
}
