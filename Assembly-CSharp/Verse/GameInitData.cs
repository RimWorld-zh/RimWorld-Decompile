using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BC9 RID: 3017
	public class GameInitData
	{
		// Token: 0x04002CDA RID: 11482
		public int startingTile = -1;

		// Token: 0x04002CDB RID: 11483
		public int mapSize = 250;

		// Token: 0x04002CDC RID: 11484
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04002CDD RID: 11485
		public int startingPawnCount = -1;

		// Token: 0x04002CDE RID: 11486
		public Faction playerFaction = null;

		// Token: 0x04002CDF RID: 11487
		public Season startingSeason = Season.Undefined;

		// Token: 0x04002CE0 RID: 11488
		public bool permadeath;

		// Token: 0x04002CE1 RID: 11489
		public bool startedFromEntry = false;

		// Token: 0x04002CE2 RID: 11490
		public string gameToLoad;

		// Token: 0x04002CE3 RID: 11491
		public const int DefaultMapSize = 250;

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x0022A6DC File Offset: 0x00228ADC
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0022A70D File Offset: 0x00228B0D
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0022A71B File Offset: 0x00228B1B
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0022A744 File Offset: 0x00228B44
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

		// Token: 0x060041B7 RID: 16823 RVA: 0x0022A798 File Offset: 0x00228B98
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
