using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Site : MapParent
	{
		public string customLabel;

		public SiteCoreDef core;

		public List<SitePartDef> parts = new List<SitePartDef>();

		private bool startedCountdown;

		private bool anyEnemiesInitially;

		private Material cachedMat;

		public override string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
				{
					return this.parts[0].label;
				}
				return this.core.label;
			}
		}

		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.LeadingSiteDef.ExpandingIconTexture;
			}
		}

		public override bool TransportPodsCanLandAndGenerateMap
		{
			get
			{
				return this.core.transportPodsCanLandAndGenerateMap;
			}
		}

		public bool KnownDanger
		{
			get
			{
				return this.LeadingSiteDef.knownDanger;
			}
		}

		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.LeadingSiteDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.LeadingSiteDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.LeadingSiteDef.applyFactionColorToSiteTexture || this.LeadingSiteDef.showFactionInInspectString;
			}
		}

		private SiteDefBase LeadingSiteDef
		{
			get
			{
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
				{
					return this.parts[0];
				}
				return this.core;
			}
		}

		public override IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				Site.<>c__Iterator109 <>c__Iterator = new Site.<>c__Iterator109();
				<>c__Iterator.<>f__this = this;
				Site.<>c__Iterator109 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Defs.Look<SiteCoreDef>(ref this.core, "core");
			Scribe_Collections.Look<SitePartDef>(ref this.parts, "parts", LookMode.Def, new object[0]);
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
		}

		public override void Tick()
		{
			base.Tick();
			this.core.Worker.SiteCoreWorkerTick(this);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Worker.SitePartWorkerTick(this);
			}
			if (base.HasMap)
			{
				this.CheckStartForceExitAndRemoveMapCountdown();
			}
		}

		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			this.core.Worker.PostMapGenerate(map);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Worker.PostMapGenerate(map);
			}
			this.anyEnemiesInitially = GenHostility.AnyHostileActiveThreat(base.Map);
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = true;
			return !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		[DebuggerHidden]
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			Site.<GetFloatMenuOptions>c__Iterator10A <GetFloatMenuOptions>c__Iterator10A = new Site.<GetFloatMenuOptions>c__Iterator10A();
			<GetFloatMenuOptions>c__Iterator10A.caravan = caravan;
			<GetFloatMenuOptions>c__Iterator10A.<$>caravan = caravan;
			<GetFloatMenuOptions>c__Iterator10A.<>f__this = this;
			Site.<GetFloatMenuOptions>c__Iterator10A expr_1C = <GetFloatMenuOptions>c__Iterator10A;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Site.<GetGizmos>c__Iterator10B <GetGizmos>c__Iterator10B = new Site.<GetGizmos>c__Iterator10B();
			<GetGizmos>c__Iterator10B.<>f__this = this;
			Site.<GetGizmos>c__Iterator10B expr_0E = <GetGizmos>c__Iterator10B;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (this.startedCountdown)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreat(base.Map))
			{
				return;
			}
			this.startedCountdown = true;
			int num = Mathf.RoundToInt(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000f);
			string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(new object[]
			{
				MapParent.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
			}) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(new object[]
			{
				MapParent.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
			});
			Messages.Message(text, this, MessageSound.Benefit);
			base.StartForceExitAndRemoveMapCountdown(num);
		}
	}
}
