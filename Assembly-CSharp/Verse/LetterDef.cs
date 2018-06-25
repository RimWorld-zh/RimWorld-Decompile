using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B4D RID: 2893
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

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x002169D8 File Offset: 0x00214DD8
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

		// Token: 0x06003F64 RID: 16228 RVA: 0x00216A26 File Offset: 0x00214E26
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
