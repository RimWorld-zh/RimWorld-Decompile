using System;

namespace Verse.Sound
{
	[EditorReplaceable]
	[EditorShowClassName]
	public abstract class SoundParamSource
	{
		protected SoundParamSource()
		{
		}

		public abstract string Label { get; }

		public abstract float ValueFor(Sample samp);
	}
}
