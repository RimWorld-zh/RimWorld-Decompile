using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x04001799 RID: 6041
		private DesignateMode mode;

		// Token: 0x06002C09 RID: 11273 RVA: 0x00174B3C File Offset: 0x00172F3C
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
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x00174B8C File Offset: 0x00172F8C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x00174BA4 File Offset: 0x00172FA4
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x00174BBC File Offset: 0x00172FBC
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

		// Token: 0x06002C0D RID: 11277 RVA: 0x00174C28 File Offset: 0x00173028
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

		// Token: 0x06002C0E RID: 11278 RVA: 0x00174C74 File Offset: 0x00173074
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}
	}
}
