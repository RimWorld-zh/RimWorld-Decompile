using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B74 RID: 2932
	public class SongDef : Def
	{
		// Token: 0x04002AE1 RID: 10977
		[NoTranslate]
		public string clipPath;

		// Token: 0x04002AE2 RID: 10978
		public float volume = 1f;

		// Token: 0x04002AE3 RID: 10979
		public bool playOnMap = true;

		// Token: 0x04002AE4 RID: 10980
		public float commonality = 1f;

		// Token: 0x04002AE5 RID: 10981
		public bool tense = false;

		// Token: 0x04002AE6 RID: 10982
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04002AE7 RID: 10983
		public List<Season> allowedSeasons = null;

		// Token: 0x04002AE8 RID: 10984
		[Unsaved]
		public AudioClip clip;

		// Token: 0x06003FFA RID: 16378 RVA: 0x0021B5B4 File Offset: 0x002199B4
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

		// Token: 0x06003FFB RID: 16379 RVA: 0x0021B608 File Offset: 0x00219A08
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
