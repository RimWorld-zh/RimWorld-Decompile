using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BD RID: 1981
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x06002BE8 RID: 11240 RVA: 0x00174258 File Offset: 0x00172658
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaDelete;
			this.hotKey = KeyBindingDefOf.Misc9;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x001742D4 File Offset: 0x001726D4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00174317 File Offset: 0x00172717
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
