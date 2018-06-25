using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E5 RID: 741
	public class ThoughtStage
	{
		// Token: 0x040007AA RID: 1962
		[MustTranslate]
		public string label = null;

		// Token: 0x040007AB RID: 1963
		[MustTranslate]
		public string labelSocial = null;

		// Token: 0x040007AC RID: 1964
		[MustTranslate]
		public string description = null;

		// Token: 0x040007AD RID: 1965
		public float baseMoodEffect = 0f;

		// Token: 0x040007AE RID: 1966
		public float baseOpinionOffset = 0f;

		// Token: 0x040007AF RID: 1967
		public bool visible = true;

		// Token: 0x040007B0 RID: 1968
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel = null;

		// Token: 0x040007B1 RID: 1969
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabelSocial = null;

		// Token: 0x06000C30 RID: 3120 RVA: 0x0006C173 File Offset: 0x0006A573
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0006C190 File Offset: 0x0006A590
		public IEnumerable<string> ConfigErrors()
		{
			if (!this.labelSocial.NullOrEmpty() && this.labelSocial == this.label)
			{
				yield return "labelSocial is the same as label. labelSocial is unnecessary in this case";
			}
			if (this.baseMoodEffect != 0f && this.description.NullOrEmpty())
			{
				yield return "affects mood but doesn't have a description";
			}
			yield break;
		}
	}
}
