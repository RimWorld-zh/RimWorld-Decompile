using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B82 RID: 2946
	public abstract class SoundFilter
	{
		// Token: 0x0600401C RID: 16412
		public abstract void SetupOn(AudioSource source);

		// Token: 0x0600401D RID: 16413 RVA: 0x0021C2EC File Offset: 0x0021A6EC
		protected static T GetOrMakeFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T t = source.gameObject.GetComponent<T>();
			if (t != null)
			{
				t.enabled = true;
			}
			else
			{
				t = source.gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
