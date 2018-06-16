using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000535 RID: 1333
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060018AA RID: 6314
		public abstract int CurStageIndex { get; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00057558 File Offset: 0x00055958
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00057584 File Offset: 0x00055984
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x000575A4 File Offset: 0x000559A4
		public virtual string LabelCap
		{
			get
			{
				return this.CurStage.label.CapitalizeFirst();
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x000575CC File Offset: 0x000559CC
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x000575EC File Offset: 0x000559EC
		public string LabelCapSocial
		{
			get
			{
				string result;
				if (this.CurStage.labelSocial != null)
				{
					result = this.CurStage.labelSocial.CapitalizeFirst();
				}
				else
				{
					result = this.LabelCap;
				}
				return result;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x00057630 File Offset: 0x00055A30
		public string Description
		{
			get
			{
				string description = this.CurStage.description;
				string result;
				if (description != null)
				{
					result = description;
				}
				else
				{
					result = this.def.description;
				}
				return result;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0005766C File Offset: 0x00055A6C
		public Texture2D Icon
		{
			get
			{
				Texture2D result;
				if (this.def.Icon != null)
				{
					result = this.def.Icon;
				}
				else if (this.MoodOffset() > 0f)
				{
					result = Thought.DefaultGoodIcon;
				}
				else
				{
					result = Thought.DefaultBadIcon;
				}
				return result;
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000576C8 File Offset: 0x00055AC8
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000576DC File Offset: 0x00055ADC
		public virtual float MoodOffset()
		{
			float result;
			if (this.CurStage == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"CurStage is null while ShouldDiscard is false on ",
					this.def.defName,
					" for ",
					this.pawn
				}), false);
				result = 0f;
			}
			else
			{
				float num = this.BaseMoodOffset;
				if (this.def.effectMultiplyingStat != null)
				{
					num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00057774 File Offset: 0x00055B74
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00057797 File Offset: 0x00055B97
		public virtual void Init()
		{
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0005779C File Offset: 0x00055B9C
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x04000E99 RID: 3737
		public Pawn pawn;

		// Token: 0x04000E9A RID: 3738
		public ThoughtDef def;

		// Token: 0x04000E9B RID: 3739
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04000E9C RID: 3740
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
