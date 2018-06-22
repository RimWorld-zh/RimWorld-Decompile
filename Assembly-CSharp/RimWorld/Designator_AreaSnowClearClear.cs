using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x00174BA8 File Offset: 0x00172FA8
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
