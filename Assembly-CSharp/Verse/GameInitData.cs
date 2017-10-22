using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class GameInitData
	{
		public const int DefaultMapSize = 250;

		public int startingTile = -1;

		public int mapSize = 250;

		public List<Pawn> startingPawns = new List<Pawn>();

		public Faction playerFaction;

		public Season startingSeason;

		public bool permadeath;

		public bool startedFromEntry;

		public string gameToLoad;

		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		public override string ToString()
		{
			return "startedFromEntry: " + this.startedFromEntry + "\nstartingPawns: " + this.startingPawns.Count;
		}

		public void PrepForMapGen()
		{
			List<Pawn>.Enumerator enumerator = this.startingPawns.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Pawn current = enumerator.Current;
					current.SetFactionDirect(Faction.OfPlayer);
					PawnComponentsUtility.AddAndRemoveDynamicComponents(current, false);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			List<Pawn>.Enumerator enumerator2 = this.startingPawns.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Pawn current2 = enumerator2.Current;
					current2.workSettings.DisableAll();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				WorkTypeDef w;
				while (enumerator3.MoveNext())
				{
					w = enumerator3.Current;
					if (w.alwaysStartActive)
					{
						foreach (Pawn item in from col in this.startingPawns
						where !col.story.WorkTypeIsDisabled(w)
						select col)
						{
							item.workSettings.SetPriority(w, 3);
						}
					}
					else
					{
						bool flag = false;
						List<Pawn>.Enumerator enumerator5 = this.startingPawns.GetEnumerator();
						try
						{
							while (enumerator5.MoveNext())
							{
								Pawn current4 = enumerator5.Current;
								if (!current4.story.WorkTypeIsDisabled(w) && current4.skills.AverageOfRelevantSkillsFor(w) >= 6.0)
								{
									current4.workSettings.SetPriority(w, 3);
									flag = true;
								}
							}
						}
						finally
						{
							((IDisposable)(object)enumerator5).Dispose();
						}
						if (!flag)
						{
							IEnumerable<Pawn> source = from col in this.startingPawns
							where !col.story.WorkTypeIsDisabled(w)
							select col;
							if (source.Any())
							{
								Pawn pawn = source.InRandomOrder(null).MaxBy((Func<Pawn, float>)((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)));
								pawn.workSettings.SetPriority(w, 3);
							}
							else if (w.requireCapableColonist)
							{
								Log.Error("No colonist could do requireCapableColonist work type " + w);
							}
						}
					}
				}
			}
		}
	}
}
