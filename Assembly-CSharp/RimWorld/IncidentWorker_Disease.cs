using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032A RID: 810
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		// Token: 0x06000DD0 RID: 3536
		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00076288 File Offset: 0x00074688
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

		// Token: 0x06000DD2 RID: 3538
		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000762B8 File Offset: 0x000746B8
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

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00076348 File Offset: 0x00074748
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0007637C File Offset: 0x0007477C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			this.ApplyToPawns(this.ActualVictims(parms));
			return true;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x000763A0 File Offset: 0x000747A0
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
