using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06002C0F RID: 11279 RVA: 0x00174F5C File Offset: 0x0017335C
		public Designator_AreaSnowClearClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaSnowClearClear".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaDelete;
		}
	}
}
