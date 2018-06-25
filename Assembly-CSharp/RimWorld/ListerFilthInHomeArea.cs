using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038A RID: 906
	public class ListerFilthInHomeArea
	{
		// Token: 0x04000998 RID: 2456
		private Map map;

		// Token: 0x04000999 RID: 2457
		private List<Thing> filthInHomeArea = new List<Thing>();

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000842B8 File Offset: 0x000826B8
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x000842D4 File Offset: 0x000826D4
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000842F0 File Offset: 0x000826F0
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00084360 File Offset: 0x00082760
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00084390 File Offset: 0x00082790
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

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000843E0 File Offset: 0x000827E0
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

		// Token: 0x06000FB7 RID: 4023 RVA: 0x000844A0 File Offset: 0x000828A0
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
