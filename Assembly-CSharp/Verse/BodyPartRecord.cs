using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000AF9 RID: 2809
	public class BodyPartRecord
	{
		// Token: 0x0400275A RID: 10074
		public BodyDef body;

		// Token: 0x0400275B RID: 10075
		[TranslationHandle]
		public BodyPartDef def = null;

		// Token: 0x0400275C RID: 10076
		[MustTranslate]
		public string customLabel;

		// Token: 0x0400275D RID: 10077
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;

		// Token: 0x0400275E RID: 10078
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x0400275F RID: 10079
		public BodyPartHeight height = BodyPartHeight.Undefined;

		// Token: 0x04002760 RID: 10080
		public BodyPartDepth depth = BodyPartDepth.Undefined;

		// Token: 0x04002761 RID: 10081
		public float coverage = 1f;

		// Token: 0x04002762 RID: 10082
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x04002763 RID: 10083
		[Unsaved]
		public BodyPartRecord parent = null;

		// Token: 0x04002764 RID: 10084
		[Unsaved]
		public float coverageAbsWithChildren = 0f;

		// Token: 0x04002765 RID: 10085
		[Unsaved]
		public float coverageAbs = 0f;

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x0020CF38 File Offset: 0x0020B338
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x0020CF58 File Offset: 0x0020B358
		public string Label
		{
			get
			{
				return (!this.customLabel.NullOrEmpty()) ? this.customLabel : this.def.label;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0020CF94 File Offset: 0x0020B394
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x0020CFB4 File Offset: 0x0020B3B4
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0020CFD4 File Offset: 0x0020B3D4
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x0020CFF4 File Offset: 0x0020B3F4
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x0020D018 File Offset: 0x0020B418
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

		// Token: 0x06003E4A RID: 15946 RVA: 0x0020D086 File Offset: 0x0020B486
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x0020D098 File Offset: 0x0020B498
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

		// Token: 0x06003E4C RID: 15948 RVA: 0x0020D0E8 File Offset: 0x0020B4E8
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

		// Token: 0x06003E4D RID: 15949 RVA: 0x0020D11C File Offset: 0x0020B51C
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				yield return this.parts[i];
			}
			yield break;
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x0020D148 File Offset: 0x0020B548
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0020D16C File Offset: 0x0020B56C
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
