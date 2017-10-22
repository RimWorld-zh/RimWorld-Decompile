using System;
using System.Collections.Generic;
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
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any())
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
				if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
				{
					Color color = (!this.LeadingSiteDef.applyFactionColorToSiteTexture || base.Faction == null) ? Color.white : base.Faction.Color;
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
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any())
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
				foreach (GenStepDef extraGenStepDef in base.ExtraGenStepDefs)
				{
					yield return extraGenStepDef;
				}
				List<GenStepDef> coreGenStepDefs = this.core.ExtraGenSteps;
				for (int k = 0; k < coreGenStepDefs.Count; k++)
				{
					yield return coreGenStepDefs[k];
				}
				for (int j = 0; j < this.parts.Count; j++)
				{
					List<GenStepDef> partGenStepDefs = this.parts[j].ExtraGenSteps;
					for (int i = 0; i < partGenStepDefs.Count; i++)
					{
						yield return partGenStepDefs[i];
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", (string)null, false);
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

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			foreach (FloatMenuOption floatMenuOption2 in this.core.Worker.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption2;
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return (Gizmo)SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
		}

		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (!this.startedCountdown && !GenHostility.AnyHostileActiveThreat(base.Map))
			{
				this.startedCountdown = true;
				int num = Mathf.RoundToInt((float)(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000.0));
				string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(MapParent.GetForceExitAndRemoveMapCountdownTimeLeftString(num)) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(MapParent.GetForceExitAndRemoveMapCountdownTimeLeftString(num));
				Messages.Message(text, (WorldObject)this, MessageSound.Benefit);
				base.StartForceExitAndRemoveMapCountdown(num);
			}
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
					stringBuilder.Append("KnownSiteThreat".Translate(this.parts[0].LabelCap));
				}
				else
				{
					stringBuilder.Append("KnownSiteThreats".Translate(GenText.ToCommaList(from x in this.parts
					select x.LabelCap, true)));
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
