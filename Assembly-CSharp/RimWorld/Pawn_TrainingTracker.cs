using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051A RID: 1306
	public class Pawn_TrainingTracker : IExposable
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x000CFF88 File Offset: 0x000CE388
		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.countDecayFrom = Find.TickManager.TicksGame;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000CFFDC File Offset: 0x000CE3DC
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.wantedTrainables, "wantedTrainables", new object[0]);
			Scribe_Deep.Look<DefMap<TrainableDef, int>>(ref this.steps, "steps", new object[0]);
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.learned, "learned", new object[0]);
			Scribe_Values.Look<int>(ref this.countDecayFrom, "countDecayFrom", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnTrainingTrackerPostLoadInit(this, ref this.wantedTrainables, ref this.steps, ref this.learned);
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000D0064 File Offset: 0x000CE464
		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000D0085 File Offset: 0x000CE485
		private void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000D0098 File Offset: 0x000CE498
		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000D00BC File Offset: 0x000CE4BC
		public bool CanBeTrained(TrainableDef td)
		{
			bool result;
			if (this.steps[td] >= td.steps)
			{
				result = false;
			}
			else
			{
				List<TrainableDef> prerequisites = td.prerequisites;
				if (!prerequisites.NullOrEmpty<TrainableDef>())
				{
					for (int i = 0; i < prerequisites.Count; i++)
					{
						if (!this.HasLearned(prerequisites[i]) || this.CanBeTrained(prerequisites[i]))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000D0148 File Offset: 0x000CE548
		public bool HasLearned(TrainableDef td)
		{
			return this.learned[td];
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000D016C File Offset: 0x000CE56C
		public AcceptanceReport CanAssignToTrain(TrainableDef td)
		{
			bool flag;
			return this.CanAssignToTrain(td, out flag);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000D018C File Offset: 0x000CE58C
		public AcceptanceReport CanAssignToTrain(TrainableDef td, out bool visible)
		{
			if (this.pawn.RaceProps.untrainableTags != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.untrainableTags.Count; i++)
				{
					if (td.MatchesTag(this.pawn.RaceProps.untrainableTags[i]))
					{
						visible = false;
						return false;
					}
				}
			}
			if (this.pawn.RaceProps.trainableTags != null)
			{
				int j = 0;
				while (j < this.pawn.RaceProps.trainableTags.Count)
				{
					if (td.MatchesTag(this.pawn.RaceProps.trainableTags[j]))
					{
						if (this.pawn.BodySize < td.minBodySize)
						{
							visible = true;
							return new AcceptanceReport("CannotTrainTooSmall".Translate(new object[]
							{
								this.pawn.LabelCapNoCount
							}));
						}
						visible = true;
						return true;
					}
					else
					{
						j++;
					}
				}
			}
			AcceptanceReport result;
			if (!td.defaultTrainable)
			{
				visible = false;
				result = false;
			}
			else if (this.pawn.BodySize < td.minBodySize)
			{
				visible = true;
				result = new AcceptanceReport("CannotTrainTooSmall".Translate(new object[]
				{
					this.pawn.LabelCapNoCount
				}));
			}
			else if (this.pawn.RaceProps.trainability.intelligenceOrder < td.requiredTrainability.intelligenceOrder)
			{
				visible = true;
				result = new AcceptanceReport("CannotTrainNotSmartEnough".Translate(new object[]
				{
					td.requiredTrainability
				}));
			}
			else
			{
				visible = true;
				result = true;
			}
			return result;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000D0378 File Offset: 0x000CE778
		public TrainableDef NextTrainableToTrain()
		{
			List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
			for (int i = 0; i < trainableDefsInListOrder.Count; i++)
			{
				if (this.GetWanted(trainableDefsInListOrder[i]) && this.CanBeTrained(trainableDefsInListOrder[i]))
				{
					return trainableDefsInListOrder[i];
				}
			}
			return null;
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000D03E0 File Offset: 0x000CE7E0
		public void Train(TrainableDef td, Pawn trainer, bool complete = false)
		{
			if (complete)
			{
				this.steps[td] = td.steps;
			}
			else
			{
				DefMap<TrainableDef, int> defMap;
				(defMap = this.steps)[td] = defMap[td] + 1;
			}
			if (this.steps[td] >= td.steps)
			{
				this.learned[td] = true;
			}
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000D0448 File Offset: 0x000CE848
		public void SetWantedRecursive(TrainableDef td, bool checkOn)
		{
			this.SetWanted(td, checkOn);
			if (checkOn)
			{
				if (td.prerequisites != null)
				{
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						this.SetWantedRecursive(td.prerequisites[i], true);
					}
				}
			}
			else
			{
				IEnumerable<TrainableDef> enumerable = from t in DefDatabase<TrainableDef>.AllDefsListForReading
				where t.prerequisites != null && t.prerequisites.Contains(td)
				select t;
				foreach (TrainableDef td2 in enumerable)
				{
					this.SetWantedRecursive(td2, false);
				}
			}
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000D0534 File Offset: 0x000CE934
		public void TrainingTrackerTickRare()
		{
			if (this.pawn.Suspended)
			{
				this.countDecayFrom += 250;
			}
			else if (!this.pawn.Spawned)
			{
				this.countDecayFrom += 250;
			}
			else if (this.steps[TrainableDefOf.Tameness] == 0)
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
			}
			else if (Find.TickManager.TicksGame >= this.countDecayFrom + TrainableUtility.DegradationPeriodTicks(this.pawn.def))
			{
				TrainableDef trainableDef = (from kvp in this.steps
				where kvp.Value > 0
				select kvp.Key).Except((from kvp in this.steps
				where kvp.Value > 0 && kvp.Key.prerequisites != null
				select kvp).SelectMany((KeyValuePair<TrainableDef, int> kvp) => kvp.Key.prerequisites)).RandomElement<TrainableDef>();
				if (trainableDef == TrainableDefOf.Tameness && !TrainableUtility.TamenessCanDecay(this.pawn.def))
				{
					this.countDecayFrom = Find.TickManager.TicksGame;
				}
				else
				{
					this.countDecayFrom = Find.TickManager.TicksGame;
					DefMap<TrainableDef, int> defMap;
					TrainableDef def;
					(defMap = this.steps)[def = trainableDef] = defMap[def] - 1;
					if (this.steps[trainableDef] <= 0 && this.learned[trainableDef])
					{
						this.learned[trainableDef] = false;
						if (this.pawn.Faction == Faction.OfPlayer)
						{
							if (trainableDef == TrainableDefOf.Tameness)
							{
								this.pawn.SetFaction(null, null);
								Messages.Message("MessageAnimalReturnedWild".Translate(new object[]
								{
									this.pawn.LabelShort
								}), this.pawn, MessageTypeDefOf.NegativeEvent, true);
							}
							else
							{
								Messages.Message("MessageAnimalLostSkill".Translate(new object[]
								{
									this.pawn.LabelShort,
									trainableDef.LabelCap
								}), this.pawn, MessageTypeDefOf.NegativeEvent, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000D07C2 File Offset: 0x000CEBC2
		public void Debug_MakeDegradeHappenSoon()
		{
			this.countDecayFrom = Find.TickManager.TicksGame - TrainableUtility.DegradationPeriodTicks(this.pawn.def) - 500;
		}

		// Token: 0x04000E00 RID: 3584
		private Pawn pawn;

		// Token: 0x04000E01 RID: 3585
		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		// Token: 0x04000E02 RID: 3586
		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		// Token: 0x04000E03 RID: 3587
		private DefMap<TrainableDef, bool> learned = new DefMap<TrainableDef, bool>();

		// Token: 0x04000E04 RID: 3588
		private int countDecayFrom = 0;
	}
}
