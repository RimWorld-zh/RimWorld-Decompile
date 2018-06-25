using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000533 RID: 1331
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x04000E9A RID: 3738
		public Pawn pawn;

		// Token: 0x04000E9B RID: 3739
		public ThoughtDef def;

		// Token: 0x04000E9C RID: 3740
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04000E9D RID: 3741
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060018A5 RID: 6309
		public abstract int CurStageIndex { get; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x00057598 File Offset: 0x00055998
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x000575C4 File Offset: 0x000559C4
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x000575E4 File Offset: 0x000559E4
		public virtual string LabelCap
		{
			get
			{
				return this.CurStage.label.CapitalizeFirst();
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0005760C File Offset: 0x00055A0C
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x0005762C File Offset: 0x00055A2C
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
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00057670 File Offset: 0x00055A70
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
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x000576AC File Offset: 0x00055AAC
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

		// Token: 0x060018AD RID: 6317 RVA: 0x00057708 File Offset: 0x00055B08
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0005771C File Offset: 0x00055B1C
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

		// Token: 0x060018AF RID: 6319 RVA: 0x000577B4 File Offset: 0x00055BB4
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000577D7 File Offset: 0x00055BD7
		public virtual void Init()
		{
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x000577DC File Offset: 0x00055BDC
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}
	}
}
