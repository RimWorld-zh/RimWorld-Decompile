using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0D RID: 3085
	public class Designation : IExposable
	{
		// Token: 0x04002E1E RID: 11806
		public DesignationManager designationManager;

		// Token: 0x04002E1F RID: 11807
		public DesignationDef def;

		// Token: 0x04002E20 RID: 11808
		public LocalTargetInfo target;

		// Token: 0x04002E21 RID: 11809
		public const float ClaimedDesignationDrawAltitude = 15f;

		// Token: 0x06004367 RID: 17255 RVA: 0x00239EF0 File Offset: 0x002382F0
		public Designation()
		{
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00239EF9 File Offset: 0x002382F9
		public Designation(LocalTargetInfo target, DesignationDef def)
		{
			this.target = target;
			this.def = def;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x00239F10 File Offset: 0x00238310
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x00239F30 File Offset: 0x00238330
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x00239F4C File Offset: 0x0023834C
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

		// Token: 0x0600436C RID: 17260 RVA: 0x00239FBA File Offset: 0x002383BA
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00239FE8 File Offset: 0x002383E8
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0023A028 File Offset: 0x00238428
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

		// Token: 0x0600436F RID: 17263 RVA: 0x0023A0D3 File Offset: 0x002384D3
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0023A0E8 File Offset: 0x002384E8
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
