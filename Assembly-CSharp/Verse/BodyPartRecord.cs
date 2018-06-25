using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AFC RID: 2812
	public class BodyPartRecord
	{
		// Token: 0x04002762 RID: 10082
		public BodyDef body;

		// Token: 0x04002763 RID: 10083
		[TranslationHandle]
		public BodyPartDef def = null;

		// Token: 0x04002764 RID: 10084
		[MustTranslate]
		public string customLabel;

		// Token: 0x04002765 RID: 10085
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;

		// Token: 0x04002766 RID: 10086
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04002767 RID: 10087
		public BodyPartHeight height = BodyPartHeight.Undefined;

		// Token: 0x04002768 RID: 10088
		public BodyPartDepth depth = BodyPartDepth.Undefined;

		// Token: 0x04002769 RID: 10089
		public float coverage = 1f;

		// Token: 0x0400276A RID: 10090
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x0400276B RID: 10091
		[Unsaved]
		public BodyPartRecord parent = null;

		// Token: 0x0400276C RID: 10092
		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		// Token: 0x0400276D RID: 10093
		[Unsaved]
		public float coverageAbs = 0f;

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0020D344 File Offset: 0x0020B744
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0020D364 File Offset: 0x0020B764
		public string Label
		{
			get
			{
				return (!this.customLabel.NullOrEmpty()) ? this.customLabel : this.def.label;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0020D3A0 File Offset: 0x0020B7A0
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x0020D3C0 File Offset: 0x0020B7C0
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0020D3E0 File Offset: 0x0020B7E0
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0020D400 File Offset: 0x0020B800
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0020D424 File Offset: 0x0020B824
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

		// Token: 0x06003E4E RID: 15950 RVA: 0x0020D492 File Offset: 0x0020B892
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0020D4A4 File Offset: 0x0020B8A4
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

		// Token: 0x06003E50 RID: 15952 RVA: 0x0020D4F4 File Offset: 0x0020B8F4
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

		// Token: 0x06003E51 RID: 15953 RVA: 0x0020D528 File Offset: 0x0020B928
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				yield return this.parts[i];
			}
			yield break;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0020D554 File Offset: 0x0020B954
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0020D578 File Offset: 0x0020B978
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
