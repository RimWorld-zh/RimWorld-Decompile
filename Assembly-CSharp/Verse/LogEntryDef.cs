using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class LogEntryDef : Def
	{
		[NoTranslate]
		public string iconMiss = null;

		[NoTranslate]
		public string iconDamaged = null;

		[NoTranslate]
		public string iconDamagedFromInstigator = null;

		[Unsaved]
		public Texture2D iconMissTex = null;

		[Unsaved]
		public Texture2D iconDamagedTex = null;

		[Unsaved]
		public Texture2D iconDamagedFromInstigatorTex = null;

		public LogEntryDef()
		{
		}

		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		[CompilerGenerated]
		private void <PostLoad>m__0()
		{
			if (!this.iconMiss.NullOrEmpty())
			{
				this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
			}
			if (!this.iconDamaged.NullOrEmpty())
			{
				this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
			}
			if (!this.iconDamagedFromInstigator.NullOrEmpty())
			{
				this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
			}
		}
	}
}
