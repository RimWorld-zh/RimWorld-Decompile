using UnityEngine;

namespace Verse.Sound
{
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		public AudioClip clip;

		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			base.duration = clip.length;
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
				result = (resolvedGrain_Clip != null && (Object)resolvedGrain_Clip.clip == (Object)this.clip);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return (!((Object)this.clip == (Object)null)) ? this.clip.GetHashCode() : 0;
		}
	}
}
