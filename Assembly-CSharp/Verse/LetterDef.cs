using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4E RID: 2894
	public class LetterDef : Def
	{
		// Token: 0x040029E0 RID: 10720
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x040029E1 RID: 10721
		public Color color = Color.white;

		// Token: 0x040029E2 RID: 10722
		public Color flashColor = Color.white;

		// Token: 0x040029E3 RID: 10723
		public float flashInterval = 90f;

		// Token: 0x040029E4 RID: 10724
		public bool bounce;

		// Token: 0x040029E5 RID: 10725
		public SoundDef arriveSound;

		// Token: 0x040029E6 RID: 10726
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x040029E7 RID: 10727
		public bool pauseIfPauseOnUrgentLetter;

		// Token: 0x040029E8 RID: 10728
		[Unsaved]
		private Texture2D iconTex;

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x00216CB8 File Offset: 0x002150B8
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

		// Token: 0x06003F64 RID: 16228 RVA: 0x00216D06 File Offset: 0x00215106
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
