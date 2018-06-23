using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B75 RID: 2933
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x04002AEA RID: 10986
		[LoadAlias("clipPath")]
		public string clipFolderPath = "";

		// Token: 0x06003FFF RID: 16383 RVA: 0x0021B708 File Offset: 0x00219B08
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip folderClip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(folderClip);
			}
			yield break;
		}
	}
}
