using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B81 RID: 2945
	public abstract class SoundFilter
	{
		// Token: 0x06004021 RID: 16417
		public abstract void SetupOn(AudioSource source);

		// Token: 0x06004022 RID: 16418 RVA: 0x0021CD44 File Offset: 0x0021B144
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
