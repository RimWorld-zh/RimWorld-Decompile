using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06002C10 RID: 11280 RVA: 0x00174CF8 File Offset: 0x001730F8
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
