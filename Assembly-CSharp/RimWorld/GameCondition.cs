using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GameCondition : IExposable
	{
		public GameConditionManager gameConditionManager;

		public GameConditionDef def;

		public int startTick;

		private int duration = -1;

		private bool permanent;

		protected Map Map
		{
			get
			{
				return this.gameConditionManager.map;
			}
		}

		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public virtual float SkyGazeChanceFactor
		{
			get
			{
				return 1f;
			}
		}

		public virtual float SkyGazeJoyGainFactor
		{
			get
			{
				return 1f;
			}
		}

		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867);
					return 360000000;
				}
				return this.duration;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		public virtual string TooltipString
		{
			get
			{
				string labelCap = this.def.LabelCap;
				if (this.Permanent)
				{
					labelCap = labelCap + "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location = (this.Map == null) ? ((Find.VisibleMap == null) ? ((Find.AnyPlayerHomeMap == null) ? Vector2.zero : Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile)) : Find.WorldGrid.LongLatOf(Find.VisibleMap.Tile)) : Find.WorldGrid.LongLatOf(this.Map.Tile);
					string text = labelCap;
					labelCap = text + "\n" + "Started".Translate() + ": " + GenDate.DateFullStringAt(GenDate.TickGameToAbs(this.startTick), location);
					text = labelCap;
					labelCap = text + "\n" + "Lasted".Translate() + ": " + this.TicksPassed.ToStringTicksToPeriod(true, false, true);
				}
				labelCap += "\n";
				return labelCap + "\n" + this.def.description;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.GameConditionPostLoadInit(this);
			}
		}

		public virtual void GameConditionTick()
		{
		}

		public virtual void GameConditionDraw()
		{
		}

		public virtual void Init()
		{
		}

		public virtual void End()
		{
			if (this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		public virtual float SkyTargetLerpFactor()
		{
			return 0f;
		}

		public virtual SkyTarget? SkyTarget()
		{
			return null;
		}

		public virtual float AnimalDensityFactor()
		{
			return 1f;
		}

		public virtual float PlantDensityFactor()
		{
			return 1f;
		}

		public virtual bool AllowEnjoyableOutsideNow()
		{
			return true;
		}

		public virtual List<SkyOverlay> SkyOverlays()
		{
			return null;
		}

		public virtual void DoCellSteadyEffects(IntVec3 c)
		{
		}
	}
}
