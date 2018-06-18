using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C54 RID: 3156
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x0600456B RID: 17771 RVA: 0x00085031 File Offset: 0x00083431
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x0008503B File Offset: 0x0008343B
		public override void DrawLayer()
		{
			if (DebugViewSettings.drawThingsPrinted)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00085054 File Offset: 0x00083454
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

		// Token: 0x0600456E RID: 17774
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04002F71 RID: 12145
		protected bool requireAddToMapMesh;
	}
}
