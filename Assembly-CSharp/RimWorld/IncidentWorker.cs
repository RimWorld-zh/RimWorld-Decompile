using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A3 RID: 675
	public class IncidentWorker
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000664D4 File Offset: 0x000648D4
		public virtual float AdjustedChance
		{
			get
			{
				return this.def.baseChance;
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000664F4 File Offset: 0x000648F4
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

		// Token: 0x06000B54 RID: 2900 RVA: 0x00066720 File Offset: 0x00064B20
		protected virtual bool CanFireNowSub(IncidentParms parms)
		{
			return true;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00066738 File Offset: 0x00064B38
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

		// Token: 0x06000B56 RID: 2902 RVA: 0x00066804 File Offset: 0x00064C04
		protected virtual bool TryExecuteWorker(IncidentParms parms)
		{
			Log.Error("Unimplemented incident " + this, false);
			return false;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0006682C File Offset: 0x00064C2C
		protected void SendStandardLetter()
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.", false);
			}
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, null);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0006689C File Offset: 0x00064C9C
		protected void SendStandardLetter(GlobalTargetInfo target, Faction relatedFaction = null, params string[] textArgs)
		{
			if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.", false);
			}
			string text = string.Format(this.def.letterText, textArgs).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, target, relatedFaction, null);
		}

		// Token: 0x04000635 RID: 1589
		public IncidentDef def;
	}
}
