using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBE RID: 3518
	public class AudioSourcePool
	{
		// Token: 0x06004E78 RID: 20088 RVA: 0x0028F756 File Offset: 0x0028DB56
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x0028F778 File Offset: 0x0028DB78
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

		// Token: 0x0400343C RID: 13372
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x0400343D RID: 13373
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
