using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ImmunityHandler : IExposable
	{
		public Pawn pawn;

		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		private static List<HediffDef> tmpNeededImmunitiesNow = new List<HediffDef>();

		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, new object[0]);
		}

		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			float result;
			int j;
			if (!this.pawn.RaceProps.IsFlesh)
			{
				result = 0f;
			}
			else
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
						goto IL_0063;
				}
				for (j = 0; j < this.immunityList.Count; j++)
				{
					if (this.immunityList[j].hediffDef == diseaseDef)
						goto IL_009e;
				}
				result = 1f;
			}
			goto IL_00eb;
			IL_009e:
			result = Mathf.Lerp(1f, 0f, (float)(this.immunityList[j].immunity / 0.60000002384185791));
			goto IL_00eb;
			IL_0063:
			result = 0f;
			goto IL_00eb;
			IL_00eb:
			return result;
		}

		public float GetImmunity(HediffDef def)
		{
			int num = 0;
			float result;
			while (true)
			{
				if (num < this.immunityList.Count)
				{
					ImmunityRecord immunityRecord = this.immunityList[num];
					if (immunityRecord.hediffDef == def)
					{
						result = immunityRecord.immunity;
						break;
					}
					num++;
					continue;
				}
				result = 0f;
				break;
			}
			return result;
		}

		internal void ImmunityHandlerTick()
		{
			List<HediffDef> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i]);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTick(this.pawn, firstHediffOfDef != null, firstHediffOfDef);
				if (firstHediffOfDef == null && this.AnyHediffMakesFullyImmuneTo(immunityRecord.hediffDef))
				{
					immunityRecord.immunity = Mathf.Clamp(0.650000036f, immunityRecord.immunity, 1f);
				}
			}
			for (int num = this.immunityList.Count - 1; num >= 0; num--)
			{
				if (this.immunityList[num].immunity <= 0.0 && !list.Contains(this.immunityList[num].hediffDef))
				{
					this.immunityList.RemoveAt(num);
				}
			}
		}

		private List<HediffDef> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(hediffs[i].def);
				}
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						ImmunityHandler.tmpNeededImmunitiesNow.Add(curStage.makeImmuneTo[j]);
					}
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		private bool AnyHediffMakesFullyImmuneTo(HediffDef def)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					HediffStage curStage = hediffs[num].CurStage;
					if (curStage != null && curStage.makeImmuneTo != null)
					{
						for (int i = 0; i < curStage.makeImmuneTo.Count; i++)
						{
							if (curStage.makeImmuneTo[i] == def)
								goto IL_0058;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
				IL_0058:
				result = true;
				break;
			}
			return result;
		}

		private void TryAddImmunityRecord(HediffDef def)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() != null && !this.ImmunityRecordExists(def))
			{
				ImmunityRecord immunityRecord = new ImmunityRecord();
				immunityRecord.hediffDef = def;
				this.immunityList.Add(immunityRecord);
			}
		}

		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			int num = 0;
			ImmunityRecord result;
			while (true)
			{
				if (num < this.immunityList.Count)
				{
					if (this.immunityList[num].hediffDef == def)
					{
						result = this.immunityList[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}
	}
}
