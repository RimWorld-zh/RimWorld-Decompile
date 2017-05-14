using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Site : MapParent
	{
		public string customLabel;

		public SiteCoreDef core;

		public List<SitePartDef> parts = new List<SitePartDef>();

		public bool writeSiteParts;

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
				if (this.LeadingSiteDef.knownDanger)
				{
					return true;
				}
				if (this.writeSiteParts)
				{
					for (int i = 0; i < this.parts.Count; i++)
					{
						if (this.parts[i].knownDanger)
						{
							return true;
						}
					}
				}
				return false;
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
				Site.<>c__Iterator10A <>c__Iterator10A = new Site.<>c__Iterator10A();
				<>c__Iterator10A.<>f__this = this;
				Site.<>c__Iterator10A expr_0E = <>c__Iterator10A;
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
			Scribe_Values.Look<bool>(ref this.writeSiteParts, "writeSiteParts", false, false);
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
			Site.<GetFloatMenuOptions>c__Iterator10B <GetFloatMenuOptions>c__Iterator10B = new Site.<GetFloatMenuOptions>c__Iterator10B();
			<GetFloatMenuOptions>c__Iterator10B.caravan = caravan;
			<GetFloatMenuOptions>c__Iterator10B.<$>caravan = caravan;
			<GetFloatMenuOptions>c__Iterator10B.<>f__this = this;
			Site.<GetFloatMenuOptions>c__Iterator10B expr_1C = <GetFloatMenuOptions>c__Iterator10B;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Site.<GetGizmos>c__Iterator10C <GetGizmos>c__Iterator10C = new Site.<GetGizmos>c__Iterator10C();
			<GetGizmos>c__Iterator10C.<>f__this = this;
			Site.<GetGizmos>c__Iterator10C expr_0E = <GetGizmos>c__Iterator10C;
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

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.writeSiteParts)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.parts.Count == 0)
				{
					stringBuilder.Append("KnownSiteThreatsNone".Translate());
				}
				else if (this.parts.Count == 1)
				{
					stringBuilder.Append("KnownSiteThreat".Translate(new object[]
					{
						this.parts[0].LabelCap
					}));
				}
				else
				{
					StringBuilder arg_D9_0 = stringBuilder;
					string arg_D4_0 = "KnownSiteThreats";
					object[] expr_A3 = new object[1];
					expr_A3[0] = GenText.ToCommaList(from x in this.parts
					select x.LabelCap, true);
					arg_D9_0.Append(arg_D4_0.Translate(expr_A3));
				}
			}
			return stringBuilder.ToString();
		}

		public override string GetDescription()
		{
			string text = this.LeadingSiteDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}
	}
}
