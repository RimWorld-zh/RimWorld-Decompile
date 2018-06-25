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
		// Token: 0x040009A0 RID: 2464
		private Map map;

		// Token: 0x040009A1 RID: 2465
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x040009A2 RID: 2466
		private string debugOutput = "uninitialized";

		// Token: 0x06000FCA RID: 4042 RVA: 0x00084A54 File Offset: 0x00082E54
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00084A7C File Offset: 0x00082E7C
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00084A97 File Offset: 0x00082E97
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00084AA1 File Offset: 0x00082EA1
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00084AAB File Offset: 0x00082EAB
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00084AB5 File Offset: 0x00082EB5
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00084AC0 File Offset: 0x00082EC0
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

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00084B0B File Offset: 0x00082F0B
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00084B18 File Offset: 0x00082F18
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00084B5C File Offset: 0x00082F5C
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

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00084BAC File Offset: 0x00082FAC
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00084C08 File Offset: 0x00083008
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00084C34 File Offset: 0x00083034
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00084C58 File Offset: 0x00083058
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
