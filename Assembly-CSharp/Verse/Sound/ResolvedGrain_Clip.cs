using System;
using UnityEngine;

namespace Verse.Sound
{
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		public AudioClip clip;

		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
				result = (resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip);
			}
			return result;
		}

		public override int GetHashCode()
		{
			int result;
			if (this.clip == null)
			{
				result = 0;
			}
			else
			{
				result = this.clip.GetHashCode();
			}
			return result;
		}
	}
}
