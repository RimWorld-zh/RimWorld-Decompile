using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B76 RID: 2934
	public class SongDef : Def
	{
		// Token: 0x06003FF3 RID: 16371 RVA: 0x0021AD68 File Offset: 0x00219168
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

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0021ADBC File Offset: 0x002191BC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x04002ADC RID: 10972
		[NoTranslate]
		public string clipPath;

		// Token: 0x04002ADD RID: 10973
		public float volume = 1f;

		// Token: 0x04002ADE RID: 10974
		public bool playOnMap = true;

		// Token: 0x04002ADF RID: 10975
		public float commonality = 1f;

		// Token: 0x04002AE0 RID: 10976
		public bool tense = false;

		// Token: 0x04002AE1 RID: 10977
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04002AE2 RID: 10978
		public List<Season> allowedSeasons = null;

		// Token: 0x04002AE3 RID: 10979
		[Unsaved]
		public AudioClip clip;
	}
}
