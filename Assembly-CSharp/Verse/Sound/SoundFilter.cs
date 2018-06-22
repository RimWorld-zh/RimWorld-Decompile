using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B7E RID: 2942
	public abstract class SoundFilter
	{
		// Token: 0x0600401E RID: 16414
		public abstract void SetupOn(AudioSource source);

		// Token: 0x0600401F RID: 16415 RVA: 0x0021C988 File Offset: 0x0021AD88
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
