using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038A RID: 906
	public class ListerFilthInHomeArea
	{
		// Token: 0x0400099B RID: 2459
		private Map map;

		// Token: 0x0400099C RID: 2460
		private List<Thing> filthInHomeArea = new List<Thing>();

		// Token: 0x06000FB0 RID: 4016 RVA: 0x000842C8 File Offset: 0x000826C8
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x000842E4 File Offset: 0x000826E4
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00084300 File Offset: 0x00082700
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00084370 File Offset: 0x00082770
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000843A0 File Offset: 0x000827A0
		public void Notify_FilthDespawned(Filth f)
		{
			for (int i = 0; i < this.filthInHomeArea.Count; i++)
			{
				if (this.filthInHomeArea[i] == f)
				{
					this.filthInHomeArea.RemoveAt(i);
					break;
				}
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000843F0 File Offset: 0x000827F0
		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (this.map.areaManager.Home[c])
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Filth filth = thingList[i] as Filth;
					if (filth != null)
					{
						this.filthInHomeArea.Add(filth);
					}
				}
			}
			else
			{
				for (int j = this.filthInHomeArea.Count - 1; j >= 0; j--)
				{
					if (this.filthInHomeArea[j].Position == c)
					{
						this.filthInHomeArea.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000844B0 File Offset: 0x000828B0
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing thing in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(thing.ThingID + " " + thing.Position);
			}
			return stringBuilder.ToString();
		}
	}
}
