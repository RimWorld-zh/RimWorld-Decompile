using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	public class Designation : IExposable
	{
		// Token: 0x04002E17 RID: 11799
		public DesignationManager designationManager;

		// Token: 0x04002E18 RID: 11800
		public DesignationDef def;

		// Token: 0x04002E19 RID: 11801
		public LocalTargetInfo target;

		// Token: 0x04002E1A RID: 11802
		public const float ClaimedDesignationDrawAltitude = 15f;

		// Token: 0x06004367 RID: 17255 RVA: 0x00239C10 File Offset: 0x00238010
		public Designation()
		{
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00239C19 File Offset: 0x00238019
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x00239C30 File Offset: 0x00238030
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x00239C50 File Offset: 0x00238050
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x00239C6C File Offset: 0x0023806C
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

		// Token: 0x0600436C RID: 17260 RVA: 0x00239CDA File Offset: 0x002380DA
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00239D08 File Offset: 0x00238108
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00239D48 File Offset: 0x00238148
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

		// Token: 0x0600436F RID: 17263 RVA: 0x00239DF3 File Offset: 0x002381F3
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x00239E08 File Offset: 0x00238208
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
	}
}
