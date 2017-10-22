using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class FactionManager : IExposable
	{
		private List<Faction> allFactions = new List<Faction>();

		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.def.hidden
				select fa;
			}
		}

		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return (from x in this.AllFactionsVisible
				orderby x.defeated
				select x).ThenByDescending((Func<Faction, float>)((Faction fa) => fa.def.startingGoodwill.Average));
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, new object[0]);
		}

		public void Add(Faction faction)
		{
			this.allFactions.Add(faction);
		}

		public void FactionManagerTick()
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

		public void FactionsDebugDrawOnMap()
		{
			if (DebugViewSettings.drawFactions)
			{
				for (int i = 0; i < this.allFactions.Count; i++)
				{
					this.allFactions[i].DebugDrawOnMap();
				}
			}
		}

		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false)
		{
			IEnumerable<Faction> source = from x in this.AllFactions
			where x != Faction.OfPlayer && !x.def.hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated)
			select x;
			return source.TryRandomElementByWeight<Faction>((Func<Faction, float>)delegate(Faction x)
			{
				if (tryMedievalOrBetter && (int)x.def.techLevel < 3)
				{
					return 0.1f;
				}
				return 1f;
			}, out faction);
		}

		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			Faction result = default(Faction);
			if (this.AllFactions.Where((Func<Faction, bool>)delegate(Faction x)
			{
				if (!allowHidden && x.def.hidden)
				{
					goto IL_0059;
				}
				if (!allowDefeated && x.defeated)
				{
					goto IL_0059;
				}
				if (!allowNonHumanlike && !x.def.humanlikeFaction)
				{
					goto IL_0059;
				}
				int result2 = x.HostileTo(Faction.OfPlayer) ? 1 : 0;
				goto IL_005a;
				IL_005a:
				return (byte)result2 != 0;
				IL_0059:
				result2 = 0;
				goto IL_005a;
			}).TryRandomElement<Faction>(out result))
			{
				return result;
			}
			return null;
		}

		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			Faction result = default(Faction);
			if ((from x in this.AllFactions
			where !x.IsPlayer && (allowHidden || !x.def.hidden) && (allowDefeated || !x.defeated) && (allowNonHumanlike || x.def.humanlikeFaction) && !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement<Faction>(out result))
			{
				return result;
			}
			return null;
		}

		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:");
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}
	}
}
