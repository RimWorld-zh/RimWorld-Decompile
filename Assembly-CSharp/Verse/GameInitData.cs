using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class GameInitData
	{
		public int startingTile = -1;

		public int mapSize = 250;

		public List<Pawn> startingPawns = new List<Pawn>();

		public int startingPawnCount = 3;

		public Faction playerFaction;

		public Season startingSeason;

		public bool permadeath;

		public bool startedFromEntry;

		public string gameToLoad;

		public const int DefaultMapSize = 250;

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
			while (this.startingPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingPawns[this.startingPawnCount]);
				Find.WorldPawns.PassToWorld(this.startingPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				this.startingPawns.RemoveAt(this.startingPawnCount);
			}
			foreach (Pawn startingPawn in this.startingPawns)
			{
				startingPawn.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(startingPawn, false);
			}
			foreach (Pawn startingPawn2 in this.startingPawns)
			{
				startingPawn2.workSettings.DisableAll();
			}
			foreach (WorkTypeDef allDef in DefDatabase<WorkTypeDef>.AllDefs)
			{
				if (allDef.alwaysStartActive)
				{
					foreach (Pawn item in from col in this.startingPawns
					where !col.story.WorkTypeIsDisabled(allDef)
					select col)
					{
						item.workSettings.SetPriority(allDef, 3);
					}
				}
				else
				{
					bool flag = false;
					foreach (Pawn startingPawn3 in this.startingPawns)
					{
						if (!startingPawn3.story.WorkTypeIsDisabled(allDef) && startingPawn3.skills.AverageOfRelevantSkillsFor(allDef) >= 6.0)
						{
							startingPawn3.workSettings.SetPriority(allDef, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source = from col in this.startingPawns
						where !col.story.WorkTypeIsDisabled(allDef)
						select col;
						if (source.Any())
						{
							Pawn pawn = source.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(allDef));
							pawn.workSettings.SetPriority(allDef, 3);
						}
						else if (allDef.requireCapableColonist)
						{
							Log.Error("No colonist could do requireCapableColonist work type " + allDef);
						}
					}
				}
			}
		}
	}
}
