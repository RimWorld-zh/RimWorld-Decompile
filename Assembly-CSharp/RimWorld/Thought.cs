using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		public Pawn pawn;

		public ThoughtDef def;

		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);

		public abstract int CurStageIndex
		{
			get;
		}

		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		public virtual string LabelCap
		{
			get
			{
				return this.CurStage.label.CapitalizeFirst();
			}
		}

		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		public string LabelCapSocial
		{
			get
			{
				return (this.CurStage.labelSocial == null) ? this.LabelCap : this.CurStage.labelSocial.CapitalizeFirst();
			}
		}

		public string Description
		{
			get
			{
				string description = this.CurStage.description;
				return (description == null) ? this.def.description : description;
			}
		}

		public Texture2D Icon
		{
			get
			{
				return (!((Object)this.def.Icon != (Object)null)) ? ((!(this.MoodOffset() > 0.0)) ? Thought.DefaultBadIcon : Thought.DefaultGoodIcon) : this.def.Icon;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		public virtual float MoodOffset()
		{
			float result;
			if (this.CurStage == null)
			{
				Log.Error("CurStage is null while ShouldDiscard is false on " + this.def.defName + " for " + this.pawn);
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

		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		public virtual void Init()
		{
		}

		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}
	}
}
