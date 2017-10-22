using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker
	{
		public IncidentDef def = null;

		public virtual float AdjustedChance
		{
			get
			{
				return this.def.baseChance;
			}
		}

		public bool CanFireNow(IIncidentTarget target)
		{
			bool result;
			if (!this.def.TargetAllowed(target))
			{
				result = false;
			}
			else if (GenDate.DaysPassed < this.def.earliestDay)
			{
				result = false;
			}
			else if (this.def.minPopulation > 0 && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_FreeColonists.Count() < this.def.minPopulation)
			{
				result = false;
			}
			else if (Find.Storyteller.difficulty.difficulty < this.def.minDifficulty)
			{
				result = false;
			}
			else
			{
				if (this.def.allowedBiomes != null)
				{
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					if (!this.def.allowedBiomes.Contains(biome))
					{
						result = false;
						goto IL_020c;
					}
				}
				for (int i = 0; i < Find.Scenario.parts.Count; i++)
				{
					ScenPart_DisableIncident scenPart_DisableIncident = Find.Scenario.parts[i] as ScenPart_DisableIncident;
					if (scenPart_DisableIncident != null && scenPart_DisableIncident.Incident == this.def)
						goto IL_0107;
				}
				Dictionary<IncidentDef, int> lastFireTicks = target.StoryState.lastFireTicks;
				int ticksGame = Find.TickManager.TicksGame;
				int num = default(int);
				if (lastFireTicks.TryGetValue(this.def, out num))
				{
					float num2 = (float)((float)(ticksGame - num) / 60000.0);
					if (num2 < this.def.minRefireDays)
					{
						result = false;
						goto IL_020c;
					}
				}
				List<IncidentDef> refireCheckIncidents = this.def.RefireCheckIncidents;
				if (refireCheckIncidents != null)
				{
					for (int j = 0; j < refireCheckIncidents.Count; j++)
					{
						if (lastFireTicks.TryGetValue(refireCheckIncidents[j], out num))
						{
							float num3 = (float)((float)(ticksGame - num) / 60000.0);
							if (num3 < this.def.minRefireDays)
								goto IL_01d4;
						}
					}
				}
				result = ((byte)(this.CanFireNowSub(target) ? 1 : 0) != 0);
			}
			goto IL_020c;
			IL_0107:
			result = false;
			goto IL_020c;
			IL_01d4:
			result = false;
			goto IL_020c;
			IL_020c:
			return result;
		}

		protected virtual bool CanFireNowSub(IIncidentTarget target)
		{
			return true;
		}

		public bool TryExecute(IncidentParms parms)
		{
			bool flag = this.TryExecuteWorker(parms);
			if (flag && this.def.tale != null)
			{
				Pawn pawn = null;
				if (parms.target is Caravan)
				{
					pawn = (parms.target as Caravan).RandomOwner();
				}
				else if (parms.target is Map)
				{
					pawn = (parms.target as Map).mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				}
				else if (parms.target is World)
				{
					pawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_FreeColonists.RandomElementWithFallback(null);
				}
				if (pawn != null)
				{
					TaleRecorder.RecordTale(this.def.tale, pawn);
				}
			}
			return flag;
		}

		protected virtual bool TryExecuteWorker(IncidentParms parms)
		{
			Log.Error("Unimplemented incident " + this);
			return false;
		}

		protected void SendStandardLetter()
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.");
			}
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, (string)null);
		}

		protected void SendStandardLetter(GlobalTargetInfo target, params string[] textArgs)
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.");
			}
			string text = string.Format(this.def.letterText, textArgs).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, target, (string)null);
		}
	}
}
