using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBC RID: 3516
	public class AudioSourcePool
	{
		// Token: 0x04003445 RID: 13381
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x04003446 RID: 13382
		public AudioSourcePoolWorld sourcePoolWorld;

		// Token: 0x06004E8F RID: 20111 RVA: 0x00290E12 File Offset: 0x0028F212
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x00290E34 File Offset: 0x0028F234
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
