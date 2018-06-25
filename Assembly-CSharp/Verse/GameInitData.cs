using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BCA RID: 3018
	public class GameInitData
	{
		// Token: 0x04002CE1 RID: 11489
		public int startingTile = -1;

		// Token: 0x04002CE2 RID: 11490
		public int mapSize = 250;

		// Token: 0x04002CE3 RID: 11491
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04002CE4 RID: 11492
		public int startingPawnCount = -1;

		// Token: 0x04002CE5 RID: 11493
		public Faction playerFaction = null;

		// Token: 0x04002CE6 RID: 11494
		public Season startingSeason = Season.Undefined;

		// Token: 0x04002CE7 RID: 11495
		public bool permadeath;

		// Token: 0x04002CE8 RID: 11496
		public bool startedFromEntry = false;

		// Token: 0x04002CE9 RID: 11497
		public string gameToLoad;

		// Token: 0x04002CEA RID: 11498
		public const int DefaultMapSize = 250;

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x0022A9BC File Offset: 0x00228DBC
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0022A9ED File Offset: 0x00228DED
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0022A9FB File Offset: 0x00228DFB
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0022AA24 File Offset: 0x00228E24
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

		// Token: 0x060041B7 RID: 16823 RVA: 0x0022AA78 File Offset: 0x00228E78
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
	}
}
