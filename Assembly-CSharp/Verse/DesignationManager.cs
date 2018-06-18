using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C0E RID: 3086
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x06004365 RID: 17253 RVA: 0x002389C3 File Offset: 0x00236DC3
		public DesignationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x002389E0 File Offset: 0x00236DE0
		public void ExposeData()
		{
			Scribe_Collections.Look<Designation>(ref this.allDesignations, "allDesignations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					this.allDesignations[i].designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.allDesignations.Count - 1; j >= 0; j--)
				{
					TargetType targetType = this.allDesignations[j].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType == TargetType.Cell)
						{
							if (!this.allDesignations[j].target.Cell.IsValid)
							{
								Log.Error("Cell-needing designation " + this.allDesignations[j] + " had no cell target. Removing...", false);
								this.allDesignations.RemoveAt(j);
							}
						}
					}
					else if (!this.allDesignations[j].target.HasThing)
					{
						Log.Error("Thing-needing designation " + this.allDesignations[j] + " had no thing target. Removing...", false);
						this.allDesignations.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x00238B40 File Offset: 0x00236F40
		public void DrawDesignations()
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				if (!this.allDesignations[i].target.HasThing || this.allDesignations[i].target.Thing.Map == this.map)
				{
					this.allDesignations[i].DesignationDraw();
				}
			}
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00238BC0 File Offset: 0x00236FC0
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target, false);
			}
			else if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target, false);
			}
			else
			{
				if (newDes.def.targetType == TargetType.Thing)
				{
					newDes.target.Thing.SetForbidden(false, false);
				}
				this.allDesignations.Add(newDes);
				newDes.designationManager = this;
				newDes.Notify_Added();
				Map map = (!newDes.target.HasThing) ? this.map : newDes.target.Thing.Map;
				if (map != null)
				{
					MoteMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
				}
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x00238CE8 File Offset: 0x002370E8
		public Designation DesignationOn(Thing t)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x00238D44 File Offset: 0x00237144
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			Designation result;
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.", false);
				result = null;
			}
			else
			{
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					Designation designation = this.allDesignations[i];
					if (designation.target.Thing == t && designation.def == def)
					{
						return designation;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x00238DD8 File Offset: 0x002371D8
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			Designation result;
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.", false);
				result = null;
			}
			else
			{
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					Designation designation = this.allDesignations[i];
					if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
					{
						return designation;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x00238E9C File Offset: 0x0023729C
		public IEnumerable<Designation> AllDesignationsOn(Thing t)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.allDesignations[i].target.Thing == t)
				{
					yield return this.allDesignations[i];
				}
			}
			yield break;
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00238ED0 File Offset: 0x002372D0
		public IEnumerable<Designation> AllDesignationsAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation des = this.allDesignations[i];
				if ((!des.target.HasThing || des.target.Thing.Map == this.map) && des.target.Cell == c)
				{
					yield return des;
				}
			}
			yield break;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00238F04 File Offset: 0x00237304
		public bool HasMapDesignationAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!designation.target.HasThing && designation.target.Cell == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00238F74 File Offset: 0x00237374
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation des = this.allDesignations[i];
				if (des.def == def && (!des.target.HasThing || des.target.Thing.Map == this.map))
				{
					yield return des;
				}
			}
			yield break;
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x00238FA5 File Offset: 0x002373A5
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x00238FBC File Offset: 0x002373BC
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x00238FE0 File Offset: 0x002373E0
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!standardCanceling || designation.def.designateCancelable)
				{
					if (designation.target.Thing == t)
					{
						designation.Notify_Removing();
					}
				}
			}
			this.allDesignations.RemoveAll((Designation d) => (!standardCanceling || d.def.designateCancelable) && d.target.Thing == t);
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x00239084 File Offset: 0x00237484
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x002390A8 File Offset: 0x002374A8
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				if (this.allDesignations[i].def == def)
				{
					this.allDesignations[i].Notify_Removing();
					this.allDesignations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0023910C File Offset: 0x0023750C
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				Designation designation = this.allDesignations[i];
				if (cellRect.Contains(designation.target.Cell))
				{
					if (designation.def.removeIfBuildingDespawned)
					{
						this.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x04002E11 RID: 11793
		public Map map;

		// Token: 0x04002E12 RID: 11794
		public List<Designation> allDesignations = new List<Designation>();
	}
}
