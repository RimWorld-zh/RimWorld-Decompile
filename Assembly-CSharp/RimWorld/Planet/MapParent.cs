using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02000601 RID: 1537
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x04001218 RID: 4632
		private HashSet<IncidentTargetTypeDef> hibernatableIncidentTargets;

		// Token: 0x04001219 RID: 4633
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x00107CEC File Offset: 0x001060EC
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x00107D10 File Offset: 0x00106110
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x00107D28 File Offset: 0x00106128
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x00107D48 File Offset: 0x00106148
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return (this.def.mapGenerator == null) ? MapGeneratorDefOf.Encounter : this.def.mapGenerator;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x00107D84 File Offset: 0x00106184
		public virtual IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x00107DA8 File Offset: 0x001061A8
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x00107DD4 File Offset: 0x001061D4
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00107E10 File Offset: 0x00106210
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x00107E4C File Offset: 0x0010624C
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00107E87 File Offset: 0x00106287
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00107E90 File Offset: 0x00106290
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00107E9C File Offset: 0x0010629C
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00107EB5 File Offset: 0x001062B5
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00107ED9 File Offset: 0x001062D9
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00107EE8 File Offset: 0x001062E8
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

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00107F14 File Offset: 0x00106314
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

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00107F40 File Offset: 0x00106340
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

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00107F74 File Offset: 0x00106374
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

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00107FAC File Offset: 0x001063AC
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

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00107FF8 File Offset: 0x001063F8
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

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00108068 File Offset: 0x00106468
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0010807E File Offset: 0x0010647E
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x001080A4 File Offset: 0x001064A4
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
	}
}
