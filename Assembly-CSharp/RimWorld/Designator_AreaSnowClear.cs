using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x0400179D RID: 6045
		private DesignateMode mode;

		// Token: 0x06002C08 RID: 11272 RVA: 0x00174DA0 File Offset: 0x001731A0
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002C09 RID: 11273 RVA: 0x00174DF0 File Offset: 0x001731F0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x00174E08 File Offset: 0x00173208
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00174E20 File Offset: 0x00173220
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

		// Token: 0x06002C0C RID: 11276 RVA: 0x00174E8C File Offset: 0x0017328C
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

		// Token: 0x06002C0D RID: 11277 RVA: 0x00174ED8 File Offset: 0x001732D8
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}
	}
}
