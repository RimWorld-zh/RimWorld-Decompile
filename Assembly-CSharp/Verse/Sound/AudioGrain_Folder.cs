using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B78 RID: 2936
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x04002AF1 RID: 10993
		[LoadAlias("clipPath")]
		public string clipFolderPath = "";

		// Token: 0x06004002 RID: 16386 RVA: 0x0021BAC4 File Offset: 0x00219EC4
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
