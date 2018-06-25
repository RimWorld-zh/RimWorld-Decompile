using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B77 RID: 2935
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x04002AEA RID: 10986
		[LoadAlias("clipPath")]
		public string clipFolderPath = "";

		// Token: 0x06004002 RID: 16386 RVA: 0x0021B7E4 File Offset: 0x00219BE4
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
