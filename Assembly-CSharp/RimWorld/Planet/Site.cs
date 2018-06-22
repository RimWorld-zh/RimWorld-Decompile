using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000606 RID: 1542
	public class Site : MapParent
	{
		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x0010D0A8 File Offset: 0x0010B4A8
		public override string Label
		{
			get
			{
				string label;
				if (!this.customLabel.NullOrEmpty())
				{
					label = this.customLabel;
				}
				else if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
				{
					label = this.parts[0].label;
				}
				else
				{
					label = this.core.label;
				}
				return label;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0010D11C File Offset: 0x0010B51C
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSiteDef.ExpandingIconTexture;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0010D13C File Offset: 0x0010B53C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSiteDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSiteDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x0010D1C0 File Offset: 0x0010B5C0
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSiteDef.applyFactionColorToSiteTexture || this.MainSiteDef.showFactionInInspectString;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0010D1F4 File Offset: 0x0010B5F4
		private SiteDefBase MainSiteDef
		{
			get
			{
				SiteDefBase result;
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
				{
					result = this.parts[0];
				}
				else
				{
					result = this.core;
				}
				return result;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x0010D244 File Offset: 0x0010B644
		public override IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepDef g in this.<get_ExtraGenStepDefs>__BaseCallProxy0())
				{
					yield return g;
				}
				List<GenStepDef> coreGenStepDefs = this.core.ExtraGenSteps;
				for (int i = 0; i < coreGenStepDefs.Count; i++)
				{
					yield return coreGenStepDefs[i];
				}
				for (int j = 0; j < this.parts.Count; j++)
				{
					List<GenStepDef> partGenStepDefs = this.parts[j].ExtraGenSteps;
					for (int k = 0; k < partGenStepDefs.Count; k++)
					{
						yield return partGenStepDefs[k];
					}
				}
				yield break;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0010D270 File Offset: 0x0010B670
		public string ApproachOrderString
		{
			get
			{
				return (!this.MainSiteDef.approachOrderString.NullOrEmpty()) ? string.Format(this.MainSiteDef.approachOrderString, this.Label) : "ApproachSite".Translate(new object[]
				{
					this.Label
				});
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x0010D2D0 File Offset: 0x0010B6D0
		public string ApproachingReportString
		{
			get
			{
				return (!this.MainSiteDef.approachingReportString.NullOrEmpty()) ? string.Format(this.MainSiteDef.approachingReportString, this.Label) : "ApproachingSite".Translate(new object[]
				{
					this.Label
				});
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0010D330 File Offset: 0x0010B730
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Defs.Look<SiteCoreDef>(ref this.core, "core");
			Scribe_Collections.Look<SitePartDef>(ref this.parts, "parts", LookMode.Def, new object[0]);
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.writeSiteParts, "writeSiteParts", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0010D3C8 File Offset: 0x0010B7C8
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

		// Token: 0x06001F0A RID: 7946 RVA: 0x0010D434 File Offset: 0x0010B834
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
			LookTargets lookTargets = new LookTargets();
			StringBuilder stringBuilder = new StringBuilder();
			Site.tmpUsedDefs.Clear();
			Site.tmpDefs.Clear();
			Site.tmpDefs.Add(this.core);
			for (int j = 0; j < this.parts.Count; j++)
			{
				Site.tmpDefs.Add(this.parts[j]);
			}
			LetterDef letterDef = null;
			string text = null;
			for (int k = 0; k < Site.tmpDefs.Count; k++)
			{
				string text2;
				LetterDef letterDef2;
				LookTargets lookTargets2;
				string arrivedLetterPart = Site.tmpDefs[k].Worker.GetArrivedLetterPart(map, out text2, out letterDef2, out lookTargets2);
				if (arrivedLetterPart != null)
				{
					if (!Site.tmpUsedDefs.Contains(Site.tmpDefs[k]))
					{
						Site.tmpUsedDefs.Add(Site.tmpDefs[k]);
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(arrivedLetterPart);
					}
					if (text == null)
					{
						text = text2;
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets2.IsValid())
					{
						lookTargets = new LookTargets(lookTargets.targets.Concat(lookTargets2.targets));
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter(text ?? "LetterLabelPlayerEnteredNewSiteGeneric".Translate(), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, (!lookTargets.IsValid()) ? this : lookTargets, null, null);
			}
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0010D64C File Offset: 0x0010BA4C
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = true;
			return !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0010D678 File Offset: 0x0010BA78
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption f in this.<GetFloatMenuOptions>__BaseCallProxy1(caravan))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in this.core.Worker.GetFloatMenuOptions(caravan, this))
			{
				yield return f2;
			}
			yield break;
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0010D6AC File Offset: 0x0010BAAC
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy2(pods, representative))
			{
				yield return o;
			}
			foreach (FloatMenuOption o2 in this.core.Worker.GetTransportPodsFloatMenuOptions(pods, representative, this))
			{
				yield return o2;
			}
			yield break;
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0010D6E4 File Offset: 0x0010BAE4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy3())
			{
				yield return g;
			}
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
			yield break;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0010D710 File Offset: 0x0010BB10
		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (!this.startedCountdown)
			{
				if (!GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
				{
					this.startedCountdown = true;
					int num = Mathf.RoundToInt(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000f);
					string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(new object[]
					{
						TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
					}) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(new object[]
					{
						TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
					});
					Messages.Message(text, this, MessageTypeDefOf.PositiveEvent, true);
					base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown(num);
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0010D7EC File Offset: 0x0010BBEC
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSiteDef == this.MainSiteDef;
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0010D820 File Offset: 0x0010BC20
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
					StringBuilder stringBuilder2 = stringBuilder;
					string key = "KnownSiteThreats";
					object[] array = new object[1];
					array[0] = (from x in this.parts
					select x.LabelCap).ToCommaList(true);
					stringBuilder2.Append(key.Translate(array));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0010D91C File Offset: 0x0010BD1C
		public override string GetDescription()
		{
			string text = this.MainSiteDef.description;
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

		// Token: 0x04001230 RID: 4656
		public string customLabel;

		// Token: 0x04001231 RID: 4657
		public SiteCoreDef core;

		// Token: 0x04001232 RID: 4658
		public List<SitePartDef> parts = new List<SitePartDef>();

		// Token: 0x04001233 RID: 4659
		public bool writeSiteParts;

		// Token: 0x04001234 RID: 4660
		public bool factionMustRemainHostile;

		// Token: 0x04001235 RID: 4661
		private bool startedCountdown;

		// Token: 0x04001236 RID: 4662
		private bool anyEnemiesInitially;

		// Token: 0x04001237 RID: 4663
		private Material cachedMat;

		// Token: 0x04001238 RID: 4664
		private static List<SiteDefBase> tmpDefs = new List<SiteDefBase>();

		// Token: 0x04001239 RID: 4665
		private static List<SiteDefBase> tmpUsedDefs = new List<SiteDefBase>();
	}
}
