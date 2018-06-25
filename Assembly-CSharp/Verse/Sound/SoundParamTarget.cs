using System;

namespace Verse.Sound
{
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		protected SoundParamTarget()
		{
		}

		public abstract string Label { get; }

		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		public abstract void SetOn(Sample sample, float value);
	}
}
