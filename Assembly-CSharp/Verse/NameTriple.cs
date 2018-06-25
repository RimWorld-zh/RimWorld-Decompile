using System;
using System.Linq;

namespace Verse
{
	public class NameTriple : Name
	{
		[LoadAlias("first")]
		private string firstInt;

		[LoadAlias("nick")]
		private string nickInt;

		[LoadAlias("last")]
		private string lastInt;

		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");

		public NameTriple()
		{
		}

		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = nick.Trim();
			this.lastInt = last.Trim();
		}

		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		public string Nick
		{
			get
			{
				return this.nickInt;
			}
		}

		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		public override string ToStringFull
		{
			get
			{
				string result;
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					result = this.First + " " + this.Last;
				}
				else
				{
					result = string.Concat(new string[]
					{
						this.First,
						" '",
						this.Nick,
						"' ",
						this.Last
					});
				}
				return result;
			}
		}

		public override string ToStringShort
		{
			get
			{
				return this.nickInt;
			}
		}

		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		public void PostLoad()
		{
			if (this.firstInt != null)
			{
				this.firstInt = this.firstInt.Trim();
			}
			if (this.nickInt != null)
			{
				this.nickInt = this.nickInt.Trim();
			}
			if (this.lastInt != null)
			{
				this.lastInt = this.lastInt.Trim();
			}
		}

		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.", false);
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
			}
			else
			{
				if (this.First == null)
				{
					this.firstInt = "";
				}
				if (this.Last == null)
				{
					this.lastInt = "";
				}
				if (overrideLastName != null)
				{
					this.lastInt = overrideLastName;
				}
				if (this.Nick.NullOrEmpty())
				{
					if (this.Last == "")
					{
						this.nickInt = this.First;
					}
					else
					{
						if (Rand.ValueSeeded(Gen.HashCombine<string>(this.First.GetHashCode(), this.Last)) < 0.5f)
						{
							this.nickInt = this.First;
						}
						else
						{
							this.nickInt = this.Last;
						}
						this.CapitalizeNick();
					}
				}
			}
		}

		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		public static NameTriple FromString(string rawName)
		{
			NameTriple result;
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.", false);
				result = NameTriple.Invalid;
			}
			else
			{
				NameTriple nameTriple = new NameTriple();
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < rawName.Length - 1; i++)
				{
					if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
					{
						num = i;
					}
					if (rawName[i] == '\'' && rawName[i + 1] == ' ')
					{
						num2 = i;
					}
				}
				if (num == -1 || num2 == -1)
				{
					if (!rawName.Contains(' '))
					{
						nameTriple.nickInt = rawName.Trim();
					}
					else
					{
						string[] array = rawName.Split(new char[]
						{
							' '
						});
						if (array.Length == 1)
						{
							nameTriple.nickInt = array[0].Trim();
						}
						else if (array.Length == 2)
						{
							nameTriple.firstInt = array[0].Trim();
							nameTriple.lastInt = array[1].Trim();
						}
						else
						{
							nameTriple.firstInt = array[0].Trim();
							nameTriple.lastInt = "";
							for (int j = 1; j < array.Length; j++)
							{
								NameTriple nameTriple2 = nameTriple;
								nameTriple2.lastInt += array[j];
								if (j < array.Length - 1)
								{
									NameTriple nameTriple3 = nameTriple;
									nameTriple3.lastInt += " ";
								}
							}
						}
					}
				}
				else
				{
					nameTriple.firstInt = rawName.Substring(0, num).Trim();
					nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
					nameTriple.lastInt = ((num2 >= rawName.Length - 2) ? "" : rawName.Substring(num2 + 2).Trim());
				}
				result = nameTriple;
			}
			return result;
		}

		public void CapitalizeNick()
		{
			if (!this.nickInt.NullOrEmpty())
			{
				this.nickInt = char.ToUpper(this.Nick[0]) + this.Nick.Substring(1);
			}
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (!(obj is NameTriple))
			{
				result = false;
			}
			else
			{
				NameTriple nameTriple = (NameTriple)obj;
				result = (this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick);
			}
			return result;
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.First);
			seed = Gen.HashCombine<string>(seed, this.Last);
			return Gen.HashCombine<string>(seed, this.Nick);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static NameTriple()
		{
		}
	}
}
