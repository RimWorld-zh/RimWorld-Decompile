using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BA RID: 1978
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x06002BE1 RID: 11233 RVA: 0x00174034 File Offset: 0x00172434
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

		// Token: 0x06002BE2 RID: 11234 RVA: 0x001740B0 File Offset: 0x001724B0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x001740F6 File Offset: 0x001724F6
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
