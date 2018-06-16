using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x06002C0A RID: 11274 RVA: 0x00174780 File Offset: 0x00172B80
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x001747D0 File Offset: 0x00172BD0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002C0C RID: 11276 RVA: 0x001747E8 File Offset: 0x00172BE8
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00174800 File Offset: 0x00172C00
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else
			{
				bool flag = base.Map.areaManager.SnowClear[c];
				if (this.mode == DesignateMode.Add)
				{
					result = !flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x0017486C File Offset: 0x00172C6C
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowClear[c] = true;
			}
			else
			{
				base.Map.areaManager.SnowClear[c] = false;
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x001748B8 File Offset: 0x00172CB8
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x0400179B RID: 6043
		private DesignateMode mode;
	}
}
