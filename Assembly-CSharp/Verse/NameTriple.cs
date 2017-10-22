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
				return (!(this.First == this.Nick) && !(this.Last == this.Nick)) ? (this.First + " '" + this.Nick + "' " + this.Last) : (this.First + " " + this.Last);
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

		public NameTriple()
		{
		}

		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first;
			this.nickInt = nick;
			this.lastInt = last;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", (string)null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", (string)null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", (string)null, false);
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
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.");
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
						if (Rand.ValueSeeded(Gen.HashCombine(this.First.GetHashCode(), this.Last)) < 0.5)
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
			bool result;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					result = true;
					goto IL_009c;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					result = true;
					goto IL_009c;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			result = ((byte)((nameSingle != null && nameSingle.Name == this.Nick) ? 1 : 0) != 0);
			goto IL_009c;
			IL_009c:
			return result;
		}

		public static NameTriple FromString(string rawName)
		{
			NameTriple result;
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.");
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
						string[] array = rawName.Split(' ');
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
								NameTriple obj = nameTriple;
								obj.lastInt += array[j];
								if (j < array.Length - 1)
								{
									NameTriple obj2 = nameTriple;
									obj2.lastInt += " ";
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
			return this.First + " '" + this.Nick + "' " + this.Last;
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
			seed = Gen.HashCombine(seed, this.First);
			seed = Gen.HashCombine(seed, this.Last);
			return Gen.HashCombine(seed, this.Nick);
		}
	}
}
