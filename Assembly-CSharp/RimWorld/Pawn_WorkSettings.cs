using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020001B2 RID: 434
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x00053B09 File Offset: 0x00051F09
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00053B36 File Offset: 0x00051F36
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00053B6C File Offset: 0x00051F6C
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00053B90 File Offset: 0x00051F90
		public List<WorkGiver> WorkGiversInOrderNormal
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderNormal;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00053BBC File Offset: 0x00051FBC
		public List<WorkGiver> WorkGiversInOrderEmergency
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderEmerg;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00053BE8 File Offset: 0x00051FE8
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<WorkTypeDef, int>>(ref this.priorities, "priorities", new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.priorities != null)
			{
				List<WorkTypeDef> disabledWorkTypes = this.pawn.story.DisabledWorkTypes;
				for (int i = 0; i < disabledWorkTypes.Count; i++)
				{
					this.Disable(disabledWorkTypes[i]);
				}
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00053C5B File Offset: 0x0005205B
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00053C70 File Offset: 0x00052070
		public void EnableAndInitialize()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			this.priorities.SetAll(0);
			int num = 0;
			foreach (WorkTypeDef w4 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where !w.alwaysStartActive && !this.pawn.story.WorkTypeIsDisabled(w)
			orderby this.pawn.skills.AverageOfRelevantSkillsFor(w) descending
			select w)
			{
				this.SetPriority(w4, 3);
				num++;
				if (num >= 6)
				{
					break;
				}
			}
			foreach (WorkTypeDef w2 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where w.alwaysStartActive
			select w)
			{
				if (!this.pawn.story.WorkTypeIsDisabled(w2))
				{
					this.SetPriority(w2, 3);
				}
			}
			foreach (WorkTypeDef w3 in this.pawn.story.DisabledWorkTypes)
			{
				this.Disable(w3);
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00053E08 File Offset: 0x00052208
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Log.Error(this.pawn + " did not have work settings initialized.", false);
				this.EnableAndInitialize();
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00053E34 File Offset: 0x00052234
		public void SetPriority(WorkTypeDef w, int priority)
		{
			this.ConfirmInitializedDebug();
			if (priority != 0 && this.pawn.story.WorkTypeIsDisabled(w))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to change priority on disabled worktype ",
					w,
					" for pawn ",
					this.pawn
				}), false);
			}
			else
			{
				if (priority < 0 || priority > 4)
				{
					Log.Message("Trying to set work to invalid priority " + priority, false);
				}
				this.priorities[w] = priority;
				if (priority == 0)
				{
					this.pawn.mindState.Notify_WorkPriorityDisabled(w);
				}
				this.workGiversDirty = true;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00053EEC File Offset: 0x000522EC
		public int GetPriority(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			int num = this.priorities[w];
			int result;
			if (num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				result = 3;
			}
			else
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00053F34 File Offset: 0x00052334
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00053F59 File Offset: 0x00052359
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00053F6A File Offset: 0x0005236A
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00053F86 File Offset: 0x00052386
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00053F90 File Offset: 0x00052390
		public void Notify_GainedTrait()
		{
			if (this.priorities != null)
			{
				foreach (WorkTypeDef w in this.pawn.story.DisabledWorkTypes)
				{
					this.Disable(w);
				}
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0005400C File Offset: 0x0005240C
		private void CacheWorkGiversInOrder()
		{
			Pawn_WorkSettings.wtsByPrio.Clear();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 999;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				int priority = this.GetPriority(workTypeDef);
				if (priority > 0)
				{
					if (priority < num)
					{
						if (workTypeDef.workGiversByPriority.Any((WorkGiverDef wg) => !wg.emergency))
						{
							num = priority;
						}
					}
					Pawn_WorkSettings.wtsByPrio.Add(workTypeDef);
				}
			}
			Pawn_WorkSettings.wtsByPrio.InsertionSort(delegate(WorkTypeDef a, WorkTypeDef b)
			{
				float value = (float)(a.naturalPriority + (4 - this.GetPriority(a)) * 100000);
				return ((float)(b.naturalPriority + (4 - this.GetPriority(b)) * 100000)).CompareTo(value);
			});
			this.workGiversInOrderEmerg.Clear();
			for (int j = 0; j < Pawn_WorkSettings.wtsByPrio.Count; j++)
			{
				WorkTypeDef workTypeDef2 = Pawn_WorkSettings.wtsByPrio[j];
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiver worker = workTypeDef2.workGiversByPriority[k].Worker;
					if (worker.def.emergency && this.GetPriority(worker.def.workType) <= num)
					{
						this.workGiversInOrderEmerg.Add(worker);
					}
				}
			}
			this.workGiversInOrderNormal.Clear();
			for (int l = 0; l < Pawn_WorkSettings.wtsByPrio.Count; l++)
			{
				WorkTypeDef workTypeDef3 = Pawn_WorkSettings.wtsByPrio[l];
				for (int m = 0; m < workTypeDef3.workGiversByPriority.Count; m++)
				{
					WorkGiver worker2 = workTypeDef3.workGiversByPriority[m].Worker;
					if (!worker2.def.emergency || this.GetPriority(worker2.def.workType) > num)
					{
						this.workGiversInOrderNormal.Add(worker2);
					}
				}
			}
			this.workGiversDirty = false;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00054214 File Offset: 0x00052614
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WorkSettings for " + this.pawn);
			stringBuilder.AppendLine("Cached emergency WorkGivers in order:");
			for (int i = 0; i < this.WorkGiversInOrderEmergency.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					i,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderEmergency[i].def)
				}));
			}
			stringBuilder.AppendLine("Cached normal WorkGivers in order:");
			for (int j = 0; j < this.WorkGiversInOrderNormal.Count; j++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					j,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderNormal[j].def)
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0005432C File Offset: 0x0005272C
		private string DebugStringFor(WorkGiverDef wg)
		{
			return string.Concat(new object[]
			{
				"[",
				this.GetPriority(wg.workType),
				" ",
				wg.workType.defName,
				"] - ",
				wg.defName,
				" (",
				wg.priorityInType,
				")"
			});
		}

		// Token: 0x040003C4 RID: 964
		private Pawn pawn;

		// Token: 0x040003C5 RID: 965
		private DefMap<WorkTypeDef, int> priorities = null;

		// Token: 0x040003C6 RID: 966
		private bool workGiversDirty = true;

		// Token: 0x040003C7 RID: 967
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x040003C8 RID: 968
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x040003C9 RID: 969
		public const int LowestPriority = 4;

		// Token: 0x040003CA RID: 970
		public const int DefaultPriority = 3;

		// Token: 0x040003CB RID: 971
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x040003CC RID: 972
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
