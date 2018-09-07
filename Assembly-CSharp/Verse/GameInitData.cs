using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public class GameInitData
	{
		public int startingTile = -1;

		public int mapSize = 250;

		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		public int startingPawnCount = -1;

		public Faction playerFaction;

		public Season startingSeason;

		public bool permadeathChosen;

		public bool permadeath;

		public bool startedFromEntry;

		public string gameToLoad;

		public const int DefaultMapSize = 250;

		public GameInitData()
		{
		}

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
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"startedFromEntry: ",
				this.startedFromEntry,
				"\nstartingAndOptionalPawns: ",
				this.startingAndOptionalPawns.Count
			});
		}

		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				this.startingAndOptionalPawns.RemoveAt(this.startingPawnCount);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn in list)
			{
				pawn.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
			}
			foreach (Pawn pawn2 in list)
			{
				pawn2.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					WorkTypeDef w = enumerator3.Current;
					if (w.alwaysStartActive)
					{
						foreach (Pawn pawn3 in from col in list
						where !col.story.WorkTypeIsDisabled(w)
						select col)
						{
							pawn3.workSettings.SetPriority(w, 3);
						}
					}
					else
					{
						bool flag = false;
						foreach (Pawn pawn4 in list)
						{
							if (!pawn4.story.WorkTypeIsDisabled(w) && pawn4.skills.AverageOfRelevantSkillsFor(w) >= 6f)
							{
								pawn4.workSettings.SetPriority(w, 3);
								flag = true;
							}
						}
						if (!flag)
						{
							IEnumerable<Pawn> source = from col in list
							where !col.story.WorkTypeIsDisabled(w)
							select col;
							if (source.Any<Pawn>())
							{
								Pawn pawn5 = source.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w));
								pawn5.workSettings.SetPriority(w, 3);
							}
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PrepForMapGen>c__AnonStorey0
		{
			internal WorkTypeDef w;

			public <PrepForMapGen>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn col)
			{
				return !col.story.WorkTypeIsDisabled(this.w);
			}

			internal bool <>m__1(Pawn col)
			{
				return !col.story.WorkTypeIsDisabled(this.w);
			}

			internal float <>m__2(Pawn c)
			{
				return c.skills.AverageOfRelevantSkillsFor(this.w);
			}
		}
	}
}
