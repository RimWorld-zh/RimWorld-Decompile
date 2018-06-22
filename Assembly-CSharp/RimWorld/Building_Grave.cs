using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A6 RID: 1702
	public class Building_Grave : Building_Casket, IStoreSettingsParent, IAssignableBuilding, IHaulDestination
	{
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x001381D8 File Offset: 0x001365D8
		public override Graphic Graphic
		{
			get
			{
				Graphic graphic;
				if (this.HasCorpse)
				{
					if (this.def.building.fullGraveGraphicData == null)
					{
						graphic = base.Graphic;
					}
					else
					{
						if (this.cachedGraphicFull == null)
						{
							this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
						}
						graphic = this.cachedGraphicFull;
					}
				}
				else
				{
					graphic = base.Graphic;
				}
				return graphic;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x00138254 File Offset: 0x00136654
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x00138278 File Offset: 0x00136678
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x001382CC File Offset: 0x001366CC
		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				IEnumerable<Pawn> result;
				if (!base.Spawned)
				{
					result = Enumerable.Empty<Pawn>();
				}
				else
				{
					IEnumerable<Pawn> second = from Corpse x in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
					where x.InnerPawn.IsColonist
					select x.InnerPawn;
					result = base.Map.mapPawns.FreeColonistsSpawned.Concat(second);
				}
				return result;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x00138368 File Offset: 0x00136768
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				if (this.assignedPawn != null)
				{
					yield return this.assignedPawn;
				}
				yield break;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x00138394 File Offset: 0x00136794
		public int MaxAssignedPawnsCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x001383AA File Offset: 0x001367AA
		public void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave(this);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x001383B9 File Offset: 0x001367B9
		public void TryUnassignPawn(Pawn pawn)
		{
			if (pawn == this.assignedPawn)
			{
				pawn.ownership.UnclaimGrave();
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x001383D4 File Offset: 0x001367D4
		public bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x001383FC File Offset: 0x001367FC
		public bool StorageTabVisible
		{
			get
			{
				return this.assignedPawn == null && !this.HasCorpse;
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00138428 File Offset: 0x00136828
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00138444 File Offset: 0x00136844
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0013846C File Offset: 0x0013686C
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x001384BC File Offset: 0x001368BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x001384DF File Offset: 0x001368DF
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0013850C File Offset: 0x0013690C
		public virtual void Notify_CorpseBuried(Pawn worker)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active)
			{
				comp.JustCreatedBy(worker);
				comp.InitializeArt(this.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
			worker.records.Increment(RecordDefOf.CorpsesBuried);
			TaleRecorder.RecordTale(TaleDefOf.BuriedCorpse, new object[]
			{
				worker,
				(this.Corpse == null) ? null : this.Corpse.InnerPawn
			});
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x001385A8 File Offset: 0x001369A8
		public override bool Accepts(Thing thing)
		{
			bool result;
			if (!base.Accepts(thing))
			{
				result = false;
			}
			else if (this.HasCorpse)
			{
				result = false;
			}
			else
			{
				if (this.assignedPawn != null)
				{
					Corpse corpse = thing as Corpse;
					if (corpse == null)
					{
						return false;
					}
					if (corpse.InnerPawn != this.assignedPawn)
					{
						return false;
					}
				}
				else if (!this.storageSettings.AllowedToAccept(thing))
				{
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0013863C File Offset: 0x00136A3C
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null)
				{
					if (corpse.InnerPawn.ownership.AssignedGrave != this)
					{
						corpse.InnerPawn.ownership.UnclaimGrave();
					}
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x001386D0 File Offset: 0x00136AD0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.StorageTabVisible)
			{
				foreach (Gizmo g2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.storageSettings))
				{
					yield return g2;
				}
			}
			if (!this.HasCorpse)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandGraveAssignColonistLabel".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
					defaultDesc = "CommandGraveAssignColonistDesc".Translate(),
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
					},
					hotKey = KeyBindingDefOf.Misc3
				};
			}
			yield break;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x001386FC File Offset: 0x00136AFC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.HasCorpse)
			{
				if (base.Tile != -1)
				{
					string text = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
					stringBuilder.AppendLine();
					stringBuilder.Append("DiedOn".Translate(new object[]
					{
						text
					}));
				}
			}
			else if (this.assignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.assignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001430 RID: 5168
		private StorageSettings storageSettings = null;

		// Token: 0x04001431 RID: 5169
		private Graphic cachedGraphicFull = null;

		// Token: 0x04001432 RID: 5170
		public Pawn assignedPawn = null;
	}
}
