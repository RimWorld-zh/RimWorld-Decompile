using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4F RID: 2895
	public class LogEntryDef : Def
	{
		// Token: 0x040029E9 RID: 10729
		[NoTranslate]
		public string iconMiss = null;

		// Token: 0x040029EA RID: 10730
		[NoTranslate]
		public string iconDamaged = null;

		// Token: 0x040029EB RID: 10731
		[NoTranslate]
		public string iconDamagedFromInstigator = null;

		// Token: 0x040029EC RID: 10732
		[Unsaved]
		public Texture2D iconMissTex = null;

		// Token: 0x040029ED RID: 10733
		[Unsaved]
		public Texture2D iconDamagedTex = null;

		// Token: 0x040029EE RID: 10734
		[Unsaved]
		public Texture2D iconDamagedFromInstigatorTex = null;

		// Token: 0x06003F66 RID: 16230 RVA: 0x00216D57 File Offset: 0x00215157
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
