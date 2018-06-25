using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B75 RID: 2933
	public class SongDef : Def
	{
		// Token: 0x04002AE8 RID: 10984
		[NoTranslate]
		public string clipPath;

		// Token: 0x04002AE9 RID: 10985
		public float volume = 1f;

		// Token: 0x04002AEA RID: 10986
		public bool playOnMap = true;

		// Token: 0x04002AEB RID: 10987
		public float commonality = 1f;

		// Token: 0x04002AEC RID: 10988
		public bool tense = false;

		// Token: 0x04002AED RID: 10989
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04002AEE RID: 10990
		public List<Season> allowedSeasons = null;

		// Token: 0x04002AEF RID: 10991
		[Unsaved]
		public AudioClip clip;

		// Token: 0x06003FFA RID: 16378 RVA: 0x0021B894 File Offset: 0x00219C94
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
			}
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x0021B8E8 File Offset: 0x00219CE8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}
	}
}
