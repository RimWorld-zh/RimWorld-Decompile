using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Pawn_TrainingTracker : IExposable
	{
		private Pawn pawn;

		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.wantedTrainables, "wantedTrainables", new object[0]);
			Scribe_Deep.Look<DefMap<TrainableDef, int>>(ref this.steps, "steps", new object[0]);
		}

		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		public void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		public bool IsCompleted(TrainableDef td)
		{
			return this.steps[td] >= td.steps;
		}

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
				for (int j = 0; j < this.pawn.RaceProps.trainableTags.Count; j++)
				{
					if (td.MatchesTag(this.pawn.RaceProps.trainableTags[j]))
						goto IL_00a3;
				}
			}
			if (!td.defaultTrainable)
			{
				visible = false;
				return false;
			}
			if (this.pawn.BodySize < td.minBodySize)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainTooSmall".Translate(this.pawn.LabelCapNoCount));
			}
			if (this.pawn.RaceProps.TrainableIntelligence.intelligenceOrder < td.requiredTrainableIntelligence.intelligenceOrder)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainNotSmartEnough".Translate(td.requiredTrainableIntelligence));
			}
			visible = true;
			return true;
			IL_00a3:
			if (this.pawn.BodySize < td.minBodySize)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainTooSmall".Translate(this.pawn.LabelCapNoCount));
			}
			visible = true;
			return true;
		}

		public TrainableDef NextTrainableToTrain()
		{
			List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
			for (int i = 0; i < trainableDefsInListOrder.Count; i++)
			{
				if (this.GetWanted(trainableDefsInListOrder[i]) && !this.IsCompleted(trainableDefsInListOrder[i]))
				{
					return trainableDefsInListOrder[i];
				}
			}
			return null;
		}

		public void Train(TrainableDef td, Pawn trainer)
		{
			DefMap<TrainableDef, int> defMap;
			DefMap<TrainableDef, int> obj = defMap = this.steps;
			TrainableDef def;
			TrainableDef def2 = def = td;
			int num = defMap[def];
			obj[def2] = num + 1;
			if (this.IsCompleted(td) && td == TrainableDefOf.Obedience)
			{
				this.pawn.playerSettings.master = trainer;
			}
		}

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
				foreach (TrainableDef item in enumerable)
				{
					this.SetWantedRecursive(item, false);
				}
			}
		}
	}
}
