using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BFC RID: 3068
	public class AreaManager : IExposable
	{
		// Token: 0x0600431A RID: 17178 RVA: 0x00238034 File Offset: 0x00236434
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600431B RID: 17179 RVA: 0x00238050 File Offset: 0x00236450
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x0600431C RID: 17180 RVA: 0x0023806C File Offset: 0x0023646C
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x0600431D RID: 17181 RVA: 0x00238088 File Offset: 0x00236488
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600431E RID: 17182 RVA: 0x002380A4 File Offset: 0x002364A4
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x002380C0 File Offset: 0x002364C0
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x002380DC File Offset: 0x002364DC
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x00238137 File Offset: 0x00236537
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00238164 File Offset: 0x00236564
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x002381A4 File Offset: 0x002365A4
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

		// Token: 0x06004324 RID: 17188 RVA: 0x002381F8 File Offset: 0x002365F8
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

		// Token: 0x06004325 RID: 17189 RVA: 0x0023825C File Offset: 0x0023665C
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

		// Token: 0x06004326 RID: 17190 RVA: 0x002382BE File Offset: 0x002366BE
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x002382EC File Offset: 0x002366EC
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0023832C File Offset: 0x0023672C
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

		// Token: 0x06004329 RID: 17193 RVA: 0x00238398 File Offset: 0x00236798
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x002383D8 File Offset: 0x002367D8
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00238420 File Offset: 0x00236820
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

		// Token: 0x04002DE6 RID: 11750
		public Map map;

		// Token: 0x04002DE7 RID: 11751
		private List<Area> areas = new List<Area>();

		// Token: 0x04002DE8 RID: 11752
		public const int MaxAllowedAreas = 10;
	}
}
