using System;
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

		protected Thought()
		{
		}

		public abstract int CurStageIndex { get; }

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

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Thought()
		{
		}
	}
}
