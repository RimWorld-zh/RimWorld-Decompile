using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E5 RID: 741
	public class ThoughtStage
	{
		// Token: 0x040007A7 RID: 1959
		[MustTranslate]
		public string label = null;

		// Token: 0x040007A8 RID: 1960
		[MustTranslate]
		public string labelSocial = null;

		// Token: 0x040007A9 RID: 1961
		[MustTranslate]
		public string description = null;

		// Token: 0x040007AA RID: 1962
		public float baseMoodEffect = 0f;

		// Token: 0x040007AB RID: 1963
		public float baseOpinionOffset = 0f;

		// Token: 0x040007AC RID: 1964
		public bool visible = true;

		// Token: 0x040007AD RID: 1965
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel = null;

		// Token: 0x040007AE RID: 1966
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabelSocial = null;

		// Token: 0x06000C31 RID: 3121 RVA: 0x0006C16B File Offset: 0x0006A56B
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0006C188 File Offset: 0x0006A588
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
