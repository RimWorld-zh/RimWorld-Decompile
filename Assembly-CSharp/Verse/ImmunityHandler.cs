using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3D RID: 3389
	public class ImmunityHandler : IExposable
	{
		// Token: 0x06004AAC RID: 19116 RVA: 0x0026F0EC File Offset: 0x0026D4EC
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0026F107 File Offset: 0x0026D507
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, new object[0]);
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0026F124 File Offset: 0x0026D524
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0026F148 File Offset: 0x0026D548
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			float result;
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
					{
						return 0f;
					}
				}
				for (int j = 0; j < this.immunityList.Count; j++)
				{
					if (this.immunityList[j].hediffDef == diseaseDef)
					{
						immunityCause = this.immunityList[j].source;
						return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
					}
				}
				result = 1f;
			}
			return result;
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0026F258 File Offset: 0x0026D658
		public float GetImmunity(HediffDef def)
		{
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					return immunityRecord.immunity;
				}
			}
			return 0f;
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0026F2B8 File Offset: 0x0026D6B8
		internal void ImmunityHandlerTick()
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
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
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0026F450 File Offset: 0x0026D850
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
				HediffStage curStage = hediff.CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
						{
							immunity = curStage.makeImmuneTo[j],
							source = hediff.def
						});
					}
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0026F560 File Offset: 0x0026D960
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0026F600 File Offset: 0x0026DA00
		private void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() != null)
			{
				if (!this.ImmunityRecordExists(def))
				{
					ImmunityRecord immunityRecord = new ImmunityRecord();
					immunityRecord.hediffDef = def;
					immunityRecord.source = source;
					this.immunityList.Add(immunityRecord);
				}
			}
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0026F650 File Offset: 0x0026DA50
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					return this.immunityList[i];
				}
			}
			return null;
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0026F6B0 File Offset: 0x0026DAB0
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x0400325F RID: 12895
		public Pawn pawn;

		// Token: 0x04003260 RID: 12896
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x04003261 RID: 12897
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02000D3E RID: 3390
		public struct ImmunityInfo
		{
			// Token: 0x04003262 RID: 12898
			public HediffDef immunity;

			// Token: 0x04003263 RID: 12899
			public HediffDef source;
		}
	}
}
