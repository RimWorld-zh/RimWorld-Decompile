using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B79 RID: 2937
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x06003FFD RID: 16381 RVA: 0x0021B06C File Offset: 0x0021946C
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip folderClip in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return new ResolvedGrain_Clip(folderClip);
			}
			yield break;
		}

		// Token: 0x04002AE5 RID: 10981
		[LoadAlias("clipPath")]
		public string clipFolderPath = "";
	}
}
