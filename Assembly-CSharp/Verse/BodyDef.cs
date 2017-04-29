using System;
using System.Collections.Generic;
using System.Diagnostics;

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

		[DebuggerHidden]
		public IEnumerable<BodyPartRecord> GetPartsWithTag(string tag)
		{
			BodyDef.<GetPartsWithTag>c__Iterator1BD <GetPartsWithTag>c__Iterator1BD = new BodyDef.<GetPartsWithTag>c__Iterator1BD();
			<GetPartsWithTag>c__Iterator1BD.tag = tag;
			<GetPartsWithTag>c__Iterator1BD.<$>tag = tag;
			<GetPartsWithTag>c__Iterator1BD.<>f__this = this;
			BodyDef.<GetPartsWithTag>c__Iterator1BD expr_1C = <GetPartsWithTag>c__Iterator1BD;
			expr_1C.$PC = -2;
			return expr_1C;
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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			BodyDef.<ConfigErrors>c__Iterator1BE <ConfigErrors>c__Iterator1BE = new BodyDef.<ConfigErrors>c__Iterator1BE();
			<ConfigErrors>c__Iterator1BE.<>f__this = this;
			BodyDef.<ConfigErrors>c__Iterator1BE expr_0E = <ConfigErrors>c__Iterator1BE;
			expr_0E.$PC = -2;
			return expr_0E;
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
				if (allParts[i].def.frostbiteVulnerability > 0f)
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
			if (num <= 0f)
			{
				num = 0f;
				Log.Warning(string.Concat(new string[]
				{
					"BodyDef ",
					this.defName,
					" has BodyPartRecord of ",
					node.def.defName,
					" whose children have more or equal total coverage than 1. This means parent can't be hit independently at all."
				}));
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
