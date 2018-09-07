using System;

namespace Verse
{
	public class RoomStatScoreStage
	{
		public float minScore = float.MinValue;

		public string label;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabel;

		public RoomStatScoreStage()
		{
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
