using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038B RID: 907
	public class ListerHaulables
	{
		// Token: 0x0400099A RID: 2458
		private Map map;

		// Token: 0x0400099B RID: 2459
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x0400099C RID: 2460
		private const int CellsPerTick = 4;

		// Token: 0x0400099D RID: 2461
		private static int groupCycleIndex = 0;

		// Token: 0x0400099E RID: 2462
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x0400099F RID: 2463
		private string debugOutput = "uninitialized";

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0008453C File Offset: 0x0008293C
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00084570 File Offset: 0x00082970
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0008458B File Offset: 0x0008298B
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00084595 File Offset: 0x00082995
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0008459F File Offset: 0x0008299F
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000845A9 File Offset: 0x000829A9
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x000845B3 File Offset: 0x000829B3
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000845BD File Offset: 0x000829BD
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000845C8 File Offset: 0x000829C8
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0008460C File Offset: 0x00082A0C
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			if (allGroupsListForReading.Count != 0)
			{
				int num = ListerHaulables.groupCycleIndex % allGroupsListForReading.Count;
				SlotGroup slotGroup = allGroupsListForReading[ListerHaulables.groupCycleIndex % allGroupsListForReading.Count];
				if (slotGroup.CellsList.Count != 0)
				{
					while (this.cellCycleIndices.Count <= num)
					{
						this.cellCycleIndices.Add(0);
					}
					if (this.cellCycleIndices[num] >= 2147473647)
					{
						this.cellCycleIndices[num] = 0;
					}
					for (int i = 0; i < 4; i++)
					{
						List<int> list;
						int index;
						(list = this.cellCycleIndices)[index = num] = list[index] + 1;
						IntVec3 c = slotGroup.CellsList[this.cellCycleIndices[num] % slotGroup.CellsList.Count];
						List<Thing> thingList = c.GetThingList(this.map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.EverHaulable)
							{
								this.Check(thingList[j]);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00084788 File Offset: 0x00082B88
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000847CC File Offset: 0x00082BCC
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00084824 File Offset: 0x00082C24
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				if (!this.haulables.Contains(t))
				{
					this.haulables.Add(t);
				}
			}
			else if (this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00084884 File Offset: 0x00082C84
		private bool ShouldBeHaulable(Thing t)
		{
			bool result;
			if (t.IsForbidden(Faction.OfPlayer))
			{
				result = false;
			}
			else
			{
				if (!t.def.alwaysHaulable)
				{
					if (!t.def.EverHaulable)
					{
						return false;
					}
					if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
					{
						return false;
					}
				}
				result = !t.IsInValidBestStorage();
			}
			return result;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00084919 File Offset: 0x00082D19
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00084945 File Offset: 0x00082D45
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00084978 File Offset: 0x00082D78
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
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
