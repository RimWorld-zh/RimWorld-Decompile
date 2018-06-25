using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038C RID: 908
	public class ListerMergeables
	{
		// Token: 0x040009A3 RID: 2467
		private Map map;

		// Token: 0x040009A4 RID: 2468
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x040009A5 RID: 2469
		private string debugOutput = "uninitialized";

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00084A64 File Offset: 0x00082E64
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00084A8C File Offset: 0x00082E8C
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00084AA7 File Offset: 0x00082EA7
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00084AB1 File Offset: 0x00082EB1
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00084ABB File Offset: 0x00082EBB
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00084AC5 File Offset: 0x00082EC5
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00084AD0 File Offset: 0x00082ED0
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			if (sg.CellsList != null)
			{
				for (int i = 0; i < sg.CellsList.Count; i++)
				{
					this.RecalcAllInCell(sg.CellsList[i]);
				}
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00084B1B File Offset: 0x00082F1B
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00084B28 File Offset: 0x00082F28
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00084B6C File Offset: 0x00082F6C
		private void Check(Thing t)
		{
			if (this.ShouldBeMergeable(t))
			{
				if (!this.mergeables.Contains(t))
				{
					this.mergeables.Add(t);
				}
			}
			else
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00084BBC File Offset: 0x00082FBC
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00084C18 File Offset: 0x00083018
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00084C44 File Offset: 0x00083044
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00084C68 File Offset: 0x00083068
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All mergeables (Count " + this.mergeables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.mergeables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}
	}
}
