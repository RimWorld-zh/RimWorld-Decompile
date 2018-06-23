using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBA RID: 3514
	public class AudioSourcePool
	{
		// Token: 0x04003445 RID: 13381
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x04003446 RID: 13382
		public AudioSourcePoolWorld sourcePoolWorld;

		// Token: 0x06004E8B RID: 20107 RVA: 0x00290CE6 File Offset: 0x0028F0E6
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x00290D08 File Offset: 0x0028F108
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
