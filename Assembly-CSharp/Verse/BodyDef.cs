using System;
using System.Collections.Generic;

namespace Verse
{
	public class BodyDef : Def
	{
		public BodyPartRecord corePart = null;

		[Unsaved]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		[Unsaved]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite = null;

		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		public IEnumerable<BodyPartRecord> GetPartsWithTag(string tag)
		{
			int i = 0;
			BodyPartRecord part;
			while (true)
			{
				if (i < this.AllParts.Count)
				{
					part = this.AllParts[i];
					if (!part.def.tags.Contains(tag))
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return part;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool HasPartWithTag(string tag)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.AllParts.Count)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[num];
					if (bodyPartRecord.def.tags.Contains(tag))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public BodyPartRecord GetPartAtIndex(int index)
		{
			return this.cachedAllParts[index];
		}

		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			throw new ArgumentException("Cannot get index of BodyPartRecord that is not in this BodyDef.");
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty())
			{
				yield return "no parts vulnerable to frostbite";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			using (List<BodyPartRecord>.Enumerator enumerator2 = this.AllParts.GetEnumerator())
			{
				BodyPartRecord part;
				while (true)
				{
					if (enumerator2.MoveNext())
					{
						part = enumerator2.Current;
						if (part.def.isConceptual && part.coverageAbs != 0.0)
							break;
						continue;
					}
					yield break;
				}
				yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", part);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_01b8:
			/*Error near IL_01b9: Unexpected return in MoveNext()*/;
		}

		public override void ResolveReferences()
		{
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0.0)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		private void CacheDataRecursive(BodyPartRecord node)
		{
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (num <= 0.0)
			{
				num = 0f;
				Log.Warning("BodyDef " + base.defName + " has BodyPartRecord of " + node.def.defName + " whose children have more or equal total coverage than 1. This means parent can't be hit independently at all.");
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}
	}
}
