using System;

namespace Verse
{
	public class RoomStatScoreStage
	{
		public float minScore = float.MinValue;

		public string label = null;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabel = null;

		public RoomStatScoreStage()
		{
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
