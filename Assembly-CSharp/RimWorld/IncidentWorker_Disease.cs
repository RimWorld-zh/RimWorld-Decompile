using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000328 RID: 808
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		// Token: 0x06000DCC RID: 3532
		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		// Token: 0x06000DCD RID: 3533 RVA: 0x00076138 File Offset: 0x00074538
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

		// Token: 0x06000DCE RID: 3534
		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		// Token: 0x06000DCF RID: 3535 RVA: 0x00076168 File Offset: 0x00074568
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

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000761F8 File Offset: 0x000745F8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0007622C File Offset: 0x0007462C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			this.ApplyToPawns(this.ActualVictims(parms));
			return true;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00076250 File Offset: 0x00074650
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
	}
}
