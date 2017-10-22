using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public sealed class DesignationManager : IExposable
	{
		public Map map;

		public List<Designation> allDesignations = new List<Designation>();

		public DesignationManager(Map map)
		{
			this.map = map;
		}

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
				for (int num = this.allDesignations.Count - 1; num >= 0; num--)
				{
					switch (this.allDesignations[num].def.targetType)
					{
					case TargetType.Thing:
					{
						if (!this.allDesignations[num].target.HasThing)
						{
							Log.Error("Thing-needing designation " + this.allDesignations[num] + " had no thing target. Removing...");
							this.allDesignations.RemoveAt(num);
						}
						break;
					}
					case TargetType.Cell:
					{
						if (!this.allDesignations[num].target.Cell.IsValid)
						{
							Log.Error("Cell-needing designation " + this.allDesignations[num] + " had no cell target. Removing...");
							this.allDesignations.RemoveAt(num);
						}
						break;
					}
					}
				}
			}
		}

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

		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target);
			}
			else if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target);
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

		public Designation DesignationOn(Thing t)
		{
			int num = 0;
			Designation result;
			while (true)
			{
				if (num < this.allDesignations.Count)
				{
					Designation designation = this.allDesignations[num];
					if (designation.target.Thing == t)
					{
						result = designation;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			Designation result;
			Designation designation;
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.");
				result = null;
			}
			else
			{
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					designation = this.allDesignations[i];
					if (designation.target.Thing == t && designation.def == def)
						goto IL_0061;
				}
				result = null;
			}
			goto IL_0085;
			IL_0085:
			return result;
			IL_0061:
			result = designation;
			goto IL_0085;
		}

		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			Designation result;
			Designation designation;
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.");
				result = null;
			}
			else
			{
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					designation = this.allDesignations[i];
					if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
						goto IL_0090;
				}
				result = null;
			}
			goto IL_00b4;
			IL_00b4:
			return result;
			IL_0090:
			result = designation;
			goto IL_00b4;
		}

		public IEnumerable<Designation> AllDesignationsOn(Thing t)
		{
			int count = this.allDesignations.Count;
			int i = 0;
			while (true)
			{
				if (i < count)
				{
					if (this.allDesignations[i].target.Thing != t)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return this.allDesignations[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public IEnumerable<Designation> AllDesignationsAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			int i = 0;
			Designation des;
			while (true)
			{
				if (i < count)
				{
					des = this.allDesignations[i];
					if ((!des.target.HasThing || des.target.Thing.Map == this.map) && des.target.Cell == c)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return des;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			int i = 0;
			Designation des;
			while (true)
			{
				if (i < count)
				{
					des = this.allDesignations[i];
					if (des.def == def)
					{
						if (!des.target.HasThing)
							break;
						if (des.target.Thing.Map == this.map)
							break;
					}
					i++;
					continue;
				}
				yield break;
			}
			yield return des;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if ((!standardCanceling || designation.def.designateCancelable) && designation.target.Thing == t)
				{
					designation.Notify_Removing();
				}
			}
			this.allDesignations.RemoveAll((Predicate<Designation>)((Designation d) => (!standardCanceling || d.def.designateCancelable) && d.target.Thing == t));
		}

		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			for (int num = this.allDesignations.Count - 1; num >= 0; num--)
			{
				Designation designation = this.allDesignations[num];
				if (cellRect.Contains(designation.target.Cell) && designation.def.removeIfBuildingDespawned)
				{
					this.RemoveDesignation(designation);
				}
			}
		}
	}
}
