using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02000603 RID: 1539
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x00107AD0 File Offset: 0x00105ED0
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x00107AF4 File Offset: 0x00105EF4
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x00107B0C File Offset: 0x00105F0C
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x00107B2C File Offset: 0x00105F2C
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return (this.def.mapGenerator == null) ? MapGeneratorDefOf.Encounter : this.def.mapGenerator;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x00107B68 File Offset: 0x00105F68
		public virtual IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x00107B8C File Offset: 0x00105F8C
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00107BB8 File Offset: 0x00105FB8
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00107BF4 File Offset: 0x00105FF4
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00107C30 File Offset: 0x00106030
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00107C6B File Offset: 0x0010606B
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00107C74 File Offset: 0x00106074
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00107C80 File Offset: 0x00106080
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00107C99 File Offset: 0x00106099
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00107CBD File Offset: 0x001060BD
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00107CCC File Offset: 0x001060CC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate()
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00107CF8 File Offset: 0x001060F8
		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			foreach (IncidentTargetTypeDef type in this.<AcceptedTypes>__BaseCallProxy1())
			{
				yield return type;
			}
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTypeDef type2 in this.hibernatableIncidentTargets)
				{
					yield return type2;
				}
			}
			yield break;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00107D24 File Offset: 0x00106124
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy2(caravan))
			{
				yield return o;
			}
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00107D58 File Offset: 0x00106158
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative))
			{
				yield return o;
			}
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				yield return new FloatMenuOption("LandInExistingMap".Translate(new object[]
				{
					this.Label
				}), delegate()
				{
					Map myMap = representative.parent.Map;
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
					{
						representative.TryLaunch(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this.$this, x.Cell));
					}, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00107D90 File Offset: 0x00106190
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag)
				{
					Find.WorldObjects.Remove(this);
				}
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x00107DDC File Offset: 0x001061DC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(new object[]
				{
					this.EnterCooldownDaysLeft().ToString("0.#")
				});
			}
			return text;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00107E4C File Offset: 0x0010624C
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00107E62 File Offset: 0x00106262
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00107E88 File Offset: 0x00106288
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.AllThings.OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTypeDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Token: 0x0400121B RID: 4635
		private HashSet<IncidentTargetTypeDef> hibernatableIncidentTargets;

		// Token: 0x0400121C RID: 4636
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
	}
}
