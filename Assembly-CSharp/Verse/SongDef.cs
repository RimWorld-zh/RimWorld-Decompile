using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class SongDef : Def
	{
		[NoTranslate]
		public string clipPath;

		public float volume = 1f;

		public bool playOnMap = true;

		public float commonality = 1f;

		public bool tense = false;

		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		public List<Season> allowedSeasons = null;

		[Unsaved]
		public AudioClip clip;

		public SongDef()
		{
		}

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

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		[CompilerGenerated]
		private void <ResolveReferences>m__0()
		{
			this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
		}
	}
}
