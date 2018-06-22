using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4C RID: 2892
	public class LogEntryDef : Def
	{
		// Token: 0x06003F63 RID: 16227 RVA: 0x0021699B File Offset: 0x00214D9B
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

		// Token: 0x040029E2 RID: 10722
		[NoTranslate]
		public string iconMiss = null;

		// Token: 0x040029E3 RID: 10723
		[NoTranslate]
		public string iconDamaged = null;

		// Token: 0x040029E4 RID: 10724
		[NoTranslate]
		public string iconDamagedFromInstigator = null;

		// Token: 0x040029E5 RID: 10725
		[Unsaved]
		public Texture2D iconMissTex = null;

		// Token: 0x040029E6 RID: 10726
		[Unsaved]
		public Texture2D iconDamagedTex = null;

		// Token: 0x040029E7 RID: 10727
		[Unsaved]
		public Texture2D iconDamagedFromInstigatorTex = null;
	}
}
