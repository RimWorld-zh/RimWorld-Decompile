using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C51 RID: 3153
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06004574 RID: 17780 RVA: 0x0008521D File Offset: 0x0008361D
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00085227 File Offset: 0x00083627
		public override void DrawLayer()
		{
			if (DebugViewSettings.drawThingsPrinted)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x00085240 File Offset: 0x00083640
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 c in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if (thing.def.drawerType != DrawerType.None)
					{
						if (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh)
						{
							if (thing.def.hideAtSnowDepth >= 1f || base.Map.snowGrid.GetDepth(thing.Position) <= thing.def.hideAtSnowDepth)
							{
								if (thing.Position.x == c.x && thing.Position.z == c.z)
								{
									this.TakePrintFrom(thing);
								}
							}
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06004577 RID: 17783
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04002F7B RID: 12155
		protected bool requireAddToMapMesh;
	}
}
