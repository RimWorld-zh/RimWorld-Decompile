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

		public override IntVec3 MapSizeGeneratedByTransportPodsArrival
		{
			get
			{
				return SiteCoreWorker.MapSize;
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
				if ((Object)this.cachedMat == (Object)null)
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
				using (IEnumerator<GenStepDef> enumerator = base.ExtraGenStepDefs.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						GenStepDef g = enumerator.Current;
						yield return g;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				List<GenStepDef> coreGenStepDefs = this.core.ExtraGenSteps;
				int k = 0;
				if (k < coreGenStepDefs.Count)
				{
					yield return coreGenStepDefs[k];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				int j = 0;
				List<GenStepDef> partGenStepDefs;
				int i;
				while (true)
				{
					if (j < this.parts.Count)
					{
						partGenStepDefs = this.parts[j].ExtraGenSteps;
						i = 0;
						if (i < partGenStepDefs.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return partGenStepDefs[i];
				/*Error: Unable to find new state assignment for yield return*/;
				IL_01e3:
				/*Error near IL_01e4: Unexpected return in MoveNext()*/;
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
			this.anyEnemiesInitially = GenHostility.AnyHostileActiveThreatToPlayer(base.Map);
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = true;
			return !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			using (IEnumerator<FloatMenuOption> enumerator = base.GetFloatMenuOptions(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption f2 = enumerator.Current;
					yield return f2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<FloatMenuOption> enumerator2 = this.core.Worker.GetFloatMenuOptions(caravan, this).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					FloatMenuOption f = enumerator2.Current;
					yield return f;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0166:
			/*Error near IL_0167: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!base.HasMap)
				yield break;
			if (Find.WorldSelector.SingleSelectedObject != this)
				yield break;
			yield return (Gizmo)SettleInExistingMapUtility.SettleCommand(base.Map, true);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_010d:
			/*Error near IL_010e: Unexpected return in MoveNext()*/;
		}

		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (!this.startedCountdown && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
			{
				this.startedCountdown = true;
				int num = Mathf.RoundToInt((float)(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000.0));
				string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num));
				Messages.Message(text, this, MessageTypeDefOf.PositiveEvent);
				base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown(num);
				TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, base.Map.mapPawns.FreeColonists.RandomElement());
			}
		}

		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.LeadingSiteDef == this.LeadingSiteDef;
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
