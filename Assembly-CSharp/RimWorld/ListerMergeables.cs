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
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00084904 File Offset: 0x00082D04
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0008492C File Offset: 0x00082D2C
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00084947 File Offset: 0x00082D47
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00084951 File Offset: 0x00082D51
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0008495B File Offset: 0x00082D5B
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00084965 File Offset: 0x00082D65
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00084970 File Offset: 0x00082D70
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

		// Token: 0x06000FCD RID: 4045 RVA: 0x000849BB File Offset: 0x00082DBB
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000849C8 File Offset: 0x00082DC8
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00084A0C File Offset: 0x00082E0C
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

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00084A5C File Offset: 0x00082E5C
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00084AB8 File Offset: 0x00082EB8
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00084AE4 File Offset: 0x00082EE4
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00084B08 File Offset: 0x00082F08
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

		// Token: 0x040009A0 RID: 2464
		private Map map;

		// Token: 0x040009A1 RID: 2465
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x040009A2 RID: 2466
		private string debugOutput = "uninitialized";
	}
}
