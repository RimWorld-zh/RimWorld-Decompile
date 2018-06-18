using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBD RID: 3517
	public class AudioSourcePool
	{
		// Token: 0x06004E76 RID: 20086 RVA: 0x0028F736 File Offset: 0x0028DB36
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x0028F758 File Offset: 0x0028DB58
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

		// Token: 0x0400343A RID: 13370
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x0400343B RID: 13371
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
