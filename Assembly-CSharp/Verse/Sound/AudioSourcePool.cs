using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBD RID: 3517
	public class AudioSourcePool
	{
		// Token: 0x0400344C RID: 13388
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x0400344D RID: 13389
		public AudioSourcePoolWorld sourcePoolWorld;

		// Token: 0x06004E8F RID: 20111 RVA: 0x002910F2 File Offset: 0x0028F4F2
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x00291114 File Offset: 0x0028F514
		public AudioSource GetSource(bool onCamera)
		{
			AudioSource result;
			if (onCamera)
			{
				result = this.sourcePoolCamera.GetSourceCamera();
			}
			else
			{
				result = this.sourcePoolWorld.GetSourceWorld();
			}
			return result;
		}
	}
}
