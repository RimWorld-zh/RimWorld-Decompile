using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BC RID: 1980
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x06002BE4 RID: 11236 RVA: 0x001743E8 File Offset: 0x001727E8
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

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00174464 File Offset: 0x00172864
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x001744AA File Offset: 0x001728AA
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
