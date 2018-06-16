using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038A RID: 906
	public class ListerMergeables
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00084718 File Offset: 0x00082B18
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00084740 File Offset: 0x00082B40
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0008475B File Offset: 0x00082B5B
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00084765 File Offset: 0x00082B65
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0008476F File Offset: 0x00082B6F
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00084779 File Offset: 0x00082B79
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00084784 File Offset: 0x00082B84
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

		// Token: 0x06000FCD RID: 4045 RVA: 0x000847CF File Offset: 0x00082BCF
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000847DC File Offset: 0x00082BDC
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00084820 File Offset: 0x00082C20
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

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00084870 File Offset: 0x00082C70
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000848CC File Offset: 0x00082CCC
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000848F8 File Offset: 0x00082CF8
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0008491C File Offset: 0x00082D1C
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

		// Token: 0x0400099E RID: 2462
		private Map map;

		// Token: 0x0400099F RID: 2463
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x040009A0 RID: 2464
		private string debugOutput = "uninitialized";
	}
}
