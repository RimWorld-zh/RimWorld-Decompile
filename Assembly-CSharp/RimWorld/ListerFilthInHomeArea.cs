using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class ListerFilthInHomeArea
	{
		private Map map;

		private List<Thing> filthInHomeArea = new List<Thing>();

		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 allCell in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(allCell);
			}
		}

		public void Notify_FilthSpawned(Filth f)
		{
			if (((Area)this.map.areaManager.Home)[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		public void Notify_FilthDespawned(Filth f)
		{
			int num = 0;
			while (true)
			{
				if (num < this.filthInHomeArea.Count)
				{
					if (this.filthInHomeArea[num] != f)
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			this.filthInHomeArea.RemoveAt(num);
		}

		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (((Area)this.map.areaManager.Home)[c])
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
				for (int num = this.filthInHomeArea.Count - 1; num >= 0; num--)
				{
					if (this.filthInHomeArea[num].Position == c)
					{
						this.filthInHomeArea.RemoveAt(num);
					}
				}
			}
		}

		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing item in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(item.ThingID + " " + item.Position);
			}
			return stringBuilder.ToString();
		}
	}
}
