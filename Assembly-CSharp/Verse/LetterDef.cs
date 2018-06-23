using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4B RID: 2891
	public class LetterDef : Def
	{
		// Token: 0x040029D9 RID: 10713
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x040029DA RID: 10714
		public Color color = Color.white;

		// Token: 0x040029DB RID: 10715
		public Color flashColor = Color.white;

		// Token: 0x040029DC RID: 10716
		public float flashInterval = 90f;

		// Token: 0x040029DD RID: 10717
		public bool bounce;

		// Token: 0x040029DE RID: 10718
		public SoundDef arriveSound;

		// Token: 0x040029DF RID: 10719
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x040029E0 RID: 10720
		public bool pauseIfPauseOnUrgentLetter;

		// Token: 0x040029E1 RID: 10721
		[Unsaved]
		private Texture2D iconTex;

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x002168FC File Offset: 0x00214CFC
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0021694A File Offset: 0x00214D4A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}
	}
}
