using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BD RID: 1981
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x06002BE7 RID: 11239 RVA: 0x001744BC File Offset: 0x001728BC
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

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00174538 File Offset: 0x00172938
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x0017457B File Offset: 0x0017297B
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
