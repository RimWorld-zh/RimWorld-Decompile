using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4E RID: 2894
	public class LogEntryDef : Def
	{
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

		// Token: 0x06003F66 RID: 16230 RVA: 0x00216A77 File Offset: 0x00214E77
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
	}
}
