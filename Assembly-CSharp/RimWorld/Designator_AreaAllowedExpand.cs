using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BE RID: 1982
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x06002BE6 RID: 11238 RVA: 0x00173DC8 File Offset: 0x001721C8
		public Designator_AreaAllowedExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			this.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.hotKey = KeyBindingDefOf.Misc8;
			this.tutorTag = "AreaAllowedExpand";
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x00173E44 File Offset: 0x00172244
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00173E8A File Offset: 0x0017228A
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
