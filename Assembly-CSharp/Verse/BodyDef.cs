using System;
using System.Collections.Generic;

namespace Verse
{
	public class BodyDef : Def
	{
		public BodyPartRecord corePart;

		[Unsaved]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		[Unsaved]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;

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
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				BodyPartRecord part = this.AllParts[i];
				if (part.def.tags.Contains(tag))
				{
					yield return part;
				}
			}
		}

		public bool HasPartWithTag(string tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
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
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty())
			{
				yield return "no parts vulnerable to frostbite";
			}
			List<BodyPartRecord>.Enumerator enumerator2 = this.AllParts.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					BodyPartRecord part = enumerator2.Current;
					if (part.def.isConceptual && part.coverageAbs != 0.0)
					{
						yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", part);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
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
