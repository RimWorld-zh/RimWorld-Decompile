using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class AreaManager : IExposable
	{
		public Map map;

		private List<Area> areas = new List<Area>();

		private const int MaxAllowedAreasPerMode = 5;

		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		public AreaManager(Map map)
		{
			this.map = map;
		}

		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed = default(Area_Allowed);
			this.TryMakeNewAllowed(AllowedAreaMode.Humanlike, out area_Allowed);
			this.TryMakeNewAllowed(AllowedAreaMode.Animal, out area_Allowed);
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area);
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

		public Area GetLabeled(string s)
		{
			int num = 0;
			Area result;
			while (true)
			{
				if (num < this.areas.Count)
				{
					if (this.areas[num].Label == s)
					{
						result = this.areas[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public T Get<T>() where T : Area
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.areas.Count)
				{
					T val = (T)(this.areas[num] as T);
					if (val != null)
					{
						result = val;
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		private void SortAreas()
		{
			this.areas.InsertionSort((Comparison<Area>)((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority)));
		}

		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn item in PawnsFinder.AllMapsAndWorld_Alive)
			{
				if (item.playerSettings != null)
				{
					item.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		public bool CanMakeNewAllowed(AllowedAreaMode mode)
		{
			return (from a in this.areas
			where a is Area_Allowed && ((Area_Allowed)a).mode == mode
			select a).Count() < 5;
		}

		public bool TryMakeNewAllowed(AllowedAreaMode mode, out Area_Allowed area)
		{
			bool result;
			if (!this.CanMakeNewAllowed(mode))
			{
				area = null;
				result = false;
			}
			else
			{
				area = new Area_Allowed(this, mode, (string)null);
				this.areas.Add(area);
				this.SortAreas();
				result = true;
			}
			return result;
		}
	}
}
