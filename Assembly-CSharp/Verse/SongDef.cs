using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B72 RID: 2930
	public class SongDef : Def
	{
		// Token: 0x06003FF7 RID: 16375 RVA: 0x0021B4D8 File Offset: 0x002198D8
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

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0021B52C File Offset: 0x0021992C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

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
	}
}
