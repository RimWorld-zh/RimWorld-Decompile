using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0A RID: 3082
	public class Designation : IExposable
	{
		// Token: 0x06004364 RID: 17252 RVA: 0x00239B34 File Offset: 0x00237F34
		public Designation()
		{
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x00239B3D File Offset: 0x00237F3D
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06004366 RID: 17254 RVA: 0x00239B54 File Offset: 0x00237F54
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x00239B74 File Offset: 0x00237F74
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00239B90 File Offset: 0x00237F90
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.def == DesignationDefOf.Haul && !this.target.HasThing)
				{
					Log.Error("Haul designation has no target! Deleting.", false);
					this.Delete();
				}
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x00239BFE File Offset: 0x00237FFE
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x00239C2C File Offset: 0x0023802C
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x00239C6C File Offset: 0x0023806C
		public virtual void DesignationDraw()
		{
			if (!this.target.HasThing || this.target.Thing.Spawned)
			{
				Vector3 position = default(Vector3);
				if (this.target.HasThing)
				{
					position = this.target.Thing.DrawPos;
					position.y = this.DesignationDrawAltitude;
				}
				else
				{
					position = this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
				}
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, this.def.iconMat, 0);
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x00239D17 File Offset: 0x00238117
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00239D2C File Offset: 0x0023812C
		public override string ToString()
		{
			return string.Format(string.Concat(new object[]
			{
				"(",
				this.def.defName,
				" target=",
				this.target,
				")"
			}), new object[0]);
		}

		// Token: 0x04002E17 RID: 11799
		public DesignationManager designationManager;

		// Token: 0x04002E18 RID: 11800
		public DesignationDef def;

		// Token: 0x04002E19 RID: 11801
		public LocalTargetInfo target;

		// Token: 0x04002E1A RID: 11802
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
