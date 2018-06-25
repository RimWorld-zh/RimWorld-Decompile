using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BFF RID: 3071
	public class AreaManager : IExposable
	{
		// Token: 0x04002DED RID: 11757
		public Map map;

		// Token: 0x04002DEE RID: 11758
		private List<Area> areas = new List<Area>();

		// Token: 0x04002DEF RID: 11759
		public const int MaxAllowedAreas = 10;

		// Token: 0x0600431D RID: 17181 RVA: 0x002383F0 File Offset: 0x002367F0
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600431E RID: 17182 RVA: 0x0023840C File Offset: 0x0023680C
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x00238428 File Offset: 0x00236828
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004320 RID: 17184 RVA: 0x00238444 File Offset: 0x00236844
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004321 RID: 17185 RVA: 0x00238460 File Offset: 0x00236860
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x0023847C File Offset: 0x0023687C
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00238498 File Offset: 0x00236898
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x002384F3 File Offset: 0x002368F3
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00238520 File Offset: 0x00236920
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x00238560 File Offset: 0x00236960
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area, false);
			}
			else
			{
				this.areas.Remove(area);
				this.NotifyEveryoneAreaRemoved(area);
				if (Designator_AreaAllowed.SelectedArea == area)
				{
					Designator_AreaAllowed.ClearSelectedArea();
				}
			}
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x002385B4 File Offset: 0x002369B4
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00238618 File Offset: 0x00236A18
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x0023867A File Offset: 0x00236A7A
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x002386A8 File Offset: 0x00236AA8
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x002386E8 File Offset: 0x00236AE8
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00238754 File Offset: 0x00236B54
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00238794 File Offset: 0x00236B94
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x002387DC File Offset: 0x00236BDC
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			bool result;
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				result = false;
			}
			else
			{
				area = new Area_Allowed(this, null);
				this.areas.Add(area);
				this.SortAreas();
				result = true;
			}
			return result;
		}
	}
}
