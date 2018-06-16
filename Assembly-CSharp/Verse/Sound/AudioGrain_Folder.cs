using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B79 RID: 2937
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x06003FFB RID: 16379 RVA: 0x0021AF98 File Offset: 0x00219398
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
