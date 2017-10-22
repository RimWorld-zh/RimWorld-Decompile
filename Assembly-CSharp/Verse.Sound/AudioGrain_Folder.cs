using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioGrain_Folder : AudioGrain
	{
		[LoadAlias("clipPath")]
		public string clipFolderPath = string.Empty;

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			foreach (AudioClip item in ContentFinder<AudioClip>.GetAllInFolder(this.clipFolderPath))
			{
				yield return (ResolvedGrain)new ResolvedGrain_Clip(item);
			}
		}
	}
}
