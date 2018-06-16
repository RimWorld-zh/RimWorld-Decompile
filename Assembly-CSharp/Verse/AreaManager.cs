using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C00 RID: 3072
	public class AreaManager : IExposable
	{
		// Token: 0x06004313 RID: 17171 RVA: 0x00236CCC File Offset: 0x002350CC
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06004314 RID: 17172 RVA: 0x00236CE8 File Offset: 0x002350E8
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06004315 RID: 17173 RVA: 0x00236D04 File Offset: 0x00235104
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x00236D20 File Offset: 0x00235120
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004317 RID: 17175 RVA: 0x00236D3C File Offset: 0x0023513C
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004318 RID: 17176 RVA: 0x00236D58 File Offset: 0x00235158
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x00236D74 File Offset: 0x00235174
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x00236DCF File Offset: 0x002351CF
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00236DFC File Offset: 0x002351FC
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00236E3C File Offset: 0x0023523C
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

		// Token: 0x0600431D RID: 17181 RVA: 0x00236E90 File Offset: 0x00235290
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

		// Token: 0x0600431E RID: 17182 RVA: 0x00236EF4 File Offset: 0x002352F4
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

		// Token: 0x0600431F RID: 17183 RVA: 0x00236F56 File Offset: 0x00235356
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x00236F84 File Offset: 0x00235384
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x00236FC4 File Offset: 0x002353C4
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

		// Token: 0x06004322 RID: 17186 RVA: 0x00237030 File Offset: 0x00235430
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00237070 File Offset: 0x00235470
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x002370B8 File Offset: 0x002354B8
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

		// Token: 0x04002DDE RID: 11742
		public Map map;

		// Token: 0x04002DDF RID: 11743
		private List<Area> areas = new List<Area>();

		// Token: 0x04002DE0 RID: 11744
		public const int MaxAllowedAreas = 10;
	}
}
