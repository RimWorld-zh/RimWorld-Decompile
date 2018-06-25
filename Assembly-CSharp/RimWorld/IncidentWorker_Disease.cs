using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache0;

		protected IncidentWorker_Disease()
		{
		}

		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		protected IEnumerable<Pawn> PotentialVictims(IIncidentTarget target)
		{
			return this.PotentialVictimCandidates(target).Where(delegate(Pawn p)
			{
				bool result;
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					result = false;
				}
				else
				{
					if (!this.def.diseasePartsToAffect.NullOrEmpty<BodyPartDef>())
					{
						bool flag = false;
						for (int i = 0; i < this.def.diseasePartsToAffect.Count; i++)
						{
							if (IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, this.def.diseaseIncident, this.def.diseasePartsToAffect[i]))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return false;
						}
					}
					result = p.RaceProps.IsFlesh;
				}
				return result;
			});
		}

		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		private static bool CanAddHediffToAnyPartOfDef(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord, false))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			this.ApplyToPawns(this.ActualVictims(parms));
			return true;
		}

		public void ApplyToPawns(IEnumerable<Pawn> pawns)
		{
			Dictionary<HediffDef, List<Pawn>> dictionary = new Dictionary<HediffDef, List<Pawn>>();
			foreach (Pawn pawn in pawns)
			{
				HediffDef hediffDef = null;
				if (Rand.Chance(pawn.health.immunity.DiseaseContractChanceFactor(this.def.diseaseIncident, out hediffDef, null)))
				{
					HediffGiveUtility.TryApply(pawn, this.def.diseaseIncident, this.def.diseasePartsToAffect, false, 1, null);
				}
				else if (hediffDef != null)
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = new List<Pawn>();
					}
					dictionary[hediffDef].Add(pawn);
				}
			}
			foreach (KeyValuePair<HediffDef, List<Pawn>> keyValuePair in dictionary)
			{
				if (keyValuePair.Key != this.def.diseaseIncident)
				{
					string key = "MessageBlockedHediff";
					object[] array = new object[3];
					array[0] = keyValuePair.Key.LabelCap;
					array[1] = this.def.diseaseIncident.LabelCap;
					array[2] = (from victim in keyValuePair.Value
					select victim.LabelShort).ToCommaList(true);
					Messages.Message(key.Translate(array), MessageTypeDefOf.NeutralEvent, true);
				}
			}
		}

		[CompilerGenerated]
		private bool <PotentialVictims>m__0(Pawn p)
		{
			bool result;
			if (p.ParentHolder is Building_CryptosleepCasket)
			{
				result = false;
			}
			else
			{
				if (!this.def.diseasePartsToAffect.NullOrEmpty<BodyPartDef>())
				{
					bool flag = false;
					for (int i = 0; i < this.def.diseasePartsToAffect.Count; i++)
					{
						if (IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, this.def.diseaseIncident, this.def.diseasePartsToAffect[i]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				result = p.RaceProps.IsFlesh;
			}
			return result;
		}

		[CompilerGenerated]
		private static string <ApplyToPawns>m__1(Pawn victim)
		{
			return victim.LabelShort;
		}
	}
}
