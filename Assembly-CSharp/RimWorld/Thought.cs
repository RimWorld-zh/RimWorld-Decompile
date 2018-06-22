using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000531 RID: 1329
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060018A2 RID: 6306
		public abstract int CurStageIndex { get; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x0005759C File Offset: 0x0005599C
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x000575C8 File Offset: 0x000559C8
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x000575E8 File Offset: 0x000559E8
		public virtual string LabelCap
		{
			get
			{
				return this.CurStage.label.CapitalizeFirst();
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x00057610 File Offset: 0x00055A10
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00057630 File Offset: 0x00055A30
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
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x00057674 File Offset: 0x00055A74
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
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x000576B0 File Offset: 0x00055AB0
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

		// Token: 0x060018AA RID: 6314 RVA: 0x0005770C File Offset: 0x00055B0C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00057720 File Offset: 0x00055B20
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

		// Token: 0x060018AC RID: 6316 RVA: 0x000577B8 File Offset: 0x00055BB8
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x000577DB File Offset: 0x00055BDB
		public virtual void Init()
		{
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000577E0 File Offset: 0x00055BE0
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x04000E96 RID: 3734
		public Pawn pawn;

		// Token: 0x04000E97 RID: 3735
		public ThoughtDef def;

		// Token: 0x04000E98 RID: 3736
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04000E99 RID: 3737
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
