using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C0B RID: 3083
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x0600436E RID: 17262 RVA: 0x00239D8B File Offset: 0x0023818B
		public DesignationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00239DA8 File Offset: 0x002381A8
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

		// Token: 0x06004370 RID: 17264 RVA: 0x00239F08 File Offset: 0x00238308
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

		// Token: 0x06004371 RID: 17265 RVA: 0x00239F88 File Offset: 0x00238388
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

		// Token: 0x06004372 RID: 17266 RVA: 0x0023A0B0 File Offset: 0x002384B0
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

		// Token: 0x06004373 RID: 17267 RVA: 0x0023A10C File Offset: 0x0023850C
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

		// Token: 0x06004374 RID: 17268 RVA: 0x0023A1A0 File Offset: 0x002385A0
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

		// Token: 0x06004375 RID: 17269 RVA: 0x0023A264 File Offset: 0x00238664
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

		// Token: 0x06004376 RID: 17270 RVA: 0x0023A298 File Offset: 0x00238698
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

		// Token: 0x06004377 RID: 17271 RVA: 0x0023A2CC File Offset: 0x002386CC
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

		// Token: 0x06004378 RID: 17272 RVA: 0x0023A33C File Offset: 0x0023873C
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

		// Token: 0x06004379 RID: 17273 RVA: 0x0023A36D File Offset: 0x0023876D
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0023A384 File Offset: 0x00238784
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0023A3A8 File Offset: 0x002387A8
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

		// Token: 0x0600437C RID: 17276 RVA: 0x0023A44C File Offset: 0x0023884C
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0023A470 File Offset: 0x00238870
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

		// Token: 0x0600437E RID: 17278 RVA: 0x0023A4D4 File Offset: 0x002388D4
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

		// Token: 0x04002E1B RID: 11803
		public Map map;

		// Token: 0x04002E1C RID: 11804
		public List<Designation> allDesignations = new List<Designation>();
	}
}
