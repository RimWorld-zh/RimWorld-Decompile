using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker
	{
		public IncidentDef def;

		public IncidentWorker()
		{
		}

		public virtual float AdjustedChance
		{
			get
			{
				return this.def.baseChance;
			}
		}

		public bool CanFireNow(IncidentParms parms)
		{
			bool result;
			if (!this.def.TargetAllowed(parms.target))
			{
				result = false;
			}
			else if (GenDate.DaysPassed < this.def.earliestDay)
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
					BiomeDef biome = Find.WorldGrid[parms.target.Tile].biome;
					if (!this.def.allowedBiomes.Contains(biome))
					{
						return false;
					}
				}
				Scenario scenario = Find.Scenario;
				for (int i = 0; i < scenario.parts.Count; i++)
				{
					ScenPart_DisableIncident scenPart_DisableIncident = scenario.parts[i] as ScenPart_DisableIncident;
					if (scenPart_DisableIncident != null && scenPart_DisableIncident.Incident == this.def)
					{
						return false;
					}
				}
				if (this.def.minPopulation > 0 && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>() < this.def.minPopulation)
				{
					result = false;
				}
				else
				{
					Dictionary<IncidentDef, int> lastFireTicks = parms.target.StoryState.lastFireTicks;
					int ticksGame = Find.TickManager.TicksGame;
					int num;
					if (lastFireTicks.TryGetValue(this.def, out num))
					{
						float num2 = (float)(ticksGame - num) / 60000f;
						if (num2 < this.def.minRefireDays)
						{
							return false;
						}
					}
					List<IncidentDef> refireCheckIncidents = this.def.RefireCheckIncidents;
					if (refireCheckIncidents != null)
					{
						for (int j = 0; j < refireCheckIncidents.Count; j++)
						{
							if (lastFireTicks.TryGetValue(refireCheckIncidents[j], out num))
							{
								float num3 = (float)(ticksGame - num) / 60000f;
								if (num3 < this.def.minRefireDays)
								{
									return false;
								}
							}
						}
					}
					result = this.CanFireNowSub(parms);
				}
			}
			return result;
		}

		protected virtual bool CanFireNowSub(IncidentParms parms)
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
					pawn = ((Caravan)parms.target).RandomOwner();
				}
				else if (parms.target is Map)
				{
					pawn = ((Map)parms.target).mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				}
				else if (parms.target is World)
				{
					pawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.RandomElementWithFallback(null);
				}
				if (pawn != null)
				{
					TaleRecorder.RecordTale(this.def.tale, new object[]
					{
						pawn
					});
				}
			}
			return flag;
		}

		protected virtual bool TryExecuteWorker(IncidentParms parms)
		{
			Log.Error("Unimplemented incident " + this, false);
			return false;
		}

		protected void SendStandardLetter()
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.", false);
			}
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, null);
		}

		protected void SendStandardLetter(GlobalTargetInfo target, Faction relatedFaction = null, params string[] textArgs)
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.", false);
			}
			string text = string.Format(this.def.letterText, textArgs).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, target, relatedFaction, null);
		}
	}
}
