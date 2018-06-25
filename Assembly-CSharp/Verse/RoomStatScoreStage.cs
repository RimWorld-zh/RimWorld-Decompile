using System;

namespace Verse
{
	// Token: 0x02000B6F RID: 2927
	public class RoomStatScoreStage
	{
		// Token: 0x04002AD5 RID: 10965
		public float minScore = float.MinValue;

		// Token: 0x04002AD6 RID: 10966
		public string label = null;

		// Token: 0x04002AD7 RID: 10967
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x06003FEB RID: 16363 RVA: 0x0021B377 File Offset: 0x00219777
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
