using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BCB RID: 3019
	public class GameInitData
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x00229F2C File Offset: 0x0022832C
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00229F5D File Offset: 0x0022835D
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x00229F6B File Offset: 0x0022836B
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x00229F94 File Offset: 0x00228394
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

		// Token: 0x060041B2 RID: 16818 RVA: 0x00229FE8 File Offset: 0x002283E8
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

		// Token: 0x04002CD5 RID: 11477
		public int startingTile = -1;

		// Token: 0x04002CD6 RID: 11478
		public int mapSize = 250;

		// Token: 0x04002CD7 RID: 11479
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04002CD8 RID: 11480
		public int startingPawnCount = -1;

		// Token: 0x04002CD9 RID: 11481
		public Faction playerFaction = null;

		// Token: 0x04002CDA RID: 11482
		public Season startingSeason = Season.Undefined;

		// Token: 0x04002CDB RID: 11483
		public bool permadeath;

		// Token: 0x04002CDC RID: 11484
		public bool startedFromEntry = false;

		// Token: 0x04002CDD RID: 11485
		public string gameToLoad;

		// Token: 0x04002CDE RID: 11486
		public const int DefaultMapSize = 250;
	}
}
