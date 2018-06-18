using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BE RID: 1982
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x06002BE8 RID: 11240 RVA: 0x00173E5C File Offset: 0x0017225C
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

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00173ED8 File Offset: 0x001722D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00173F1E File Offset: 0x0017231E
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
