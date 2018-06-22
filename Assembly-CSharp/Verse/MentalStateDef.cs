using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000B52 RID: 2898
	public class MentalStateDef : Def
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x00216DD8 File Offset: 0x002151D8
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.workerClass != null)
					{
						this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
						this.workerInt.def = this;
					}
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x00216E30 File Offset: 0x00215230
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x00216E50 File Offset: 0x00215250
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x00216EB1 File Offset: 0x002152B1
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00216ED0 File Offset: 0x002152D0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (!this.beginLetter.NullOrEmpty() && this.beginLetterLabel.NullOrEmpty())
			{
				yield return "no beginLetter or beginLetterLabel";
			}
			yield break;
		}

		// Token: 0x04002A01 RID: 10753
		public Type stateClass = typeof(MentalState);

		// Token: 0x04002A02 RID: 10754
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x04002A03 RID: 10755
		public MentalStateCategory category = MentalStateCategory.Undefined;

		// Token: 0x04002A04 RID: 10756
		public bool prisonersCanDo = true;

		// Token: 0x04002A05 RID: 10757
		public bool unspawnedCanDo = false;

		// Token: 0x04002A06 RID: 10758
		public bool colonistsOnly = false;

		// Token: 0x04002A07 RID: 10759
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04002A08 RID: 10760
		public bool blockNormalThoughts = false;

		// Token: 0x04002A09 RID: 10761
		public EffecterDef stateEffecter;

		// Token: 0x04002A0A RID: 10762
		public TaleDef tale;

		// Token: 0x04002A0B RID: 10763
		public bool allowBeatfire = false;

		// Token: 0x04002A0C RID: 10764
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x04002A0D RID: 10765
		public bool ignoreDrugPolicy = false;

		// Token: 0x04002A0E RID: 10766
		public float recoveryMtbDays = 1f;

		// Token: 0x04002A0F RID: 10767
		public int minTicksBeforeRecovery = 500;

		// Token: 0x04002A10 RID: 10768
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x04002A11 RID: 10769
		public bool recoverFromSleep = false;

		// Token: 0x04002A12 RID: 10770
		public ThoughtDef moodRecoveryThought;

		// Token: 0x04002A13 RID: 10771
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04002A14 RID: 10772
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04002A15 RID: 10773
		public LetterDef beginLetterDef;

		// Token: 0x04002A16 RID: 10774
		public Color nameColor = Color.green;

		// Token: 0x04002A17 RID: 10775
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x04002A18 RID: 10776
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04002A19 RID: 10777
		public bool escapingPrisonersIgnore = false;

		// Token: 0x04002A1A RID: 10778
		private MentalStateWorker workerInt = null;
	}
}
