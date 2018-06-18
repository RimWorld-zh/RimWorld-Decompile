using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x00174814 File Offset: 0x00172C14
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
		// (get) Token: 0x06002C0D RID: 11277 RVA: 0x00174864 File Offset: 0x00172C64
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002C0E RID: 11278 RVA: 0x0017487C File Offset: 0x00172C7C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x00174894 File Offset: 0x00172C94
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

		// Token: 0x06002C10 RID: 11280 RVA: 0x00174900 File Offset: 0x00172D00
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

		// Token: 0x06002C11 RID: 11281 RVA: 0x0017494C File Offset: 0x00172D4C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x0400179B RID: 6043
		private DesignateMode mode;
	}
}
