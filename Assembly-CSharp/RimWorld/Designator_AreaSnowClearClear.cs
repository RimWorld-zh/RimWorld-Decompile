using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C9 RID: 1993
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06002C13 RID: 11283 RVA: 0x001749D0 File Offset: 0x00172DD0
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
