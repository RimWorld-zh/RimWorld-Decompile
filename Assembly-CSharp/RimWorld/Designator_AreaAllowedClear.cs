using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BF RID: 1983
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x06002BEB RID: 11243 RVA: 0x00173F30 File Offset: 0x00172330
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

		// Token: 0x06002BEC RID: 11244 RVA: 0x00173FAC File Offset: 0x001723AC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x00173FEF File Offset: 0x001723EF
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
