using System;
using System.Linq;
using SoftwareCraft.Maybe;

namespace Sandbox
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var mPerson = GetById(11);

			if (mPerson.HasValue) Console.WriteLine("Huston, we have a person!");

			var fullName = mPerson.Bind(GetFullName);
			var age = mPerson.Bind(GetAgeInYears);

			Print(fullName);
			Print(age);

			fullName.Map(Console.WriteLine, () => Console.WriteLine("Unknown name."));
			age.Map(Console.WriteLine, () => Console.WriteLine("Unknown age."));
		}

		private static void Print<T>(Maybe<T> maybe)
		{
			switch (maybe)
			{
				case Some<T> some:
				{
					Console.WriteLine($"{some.Value}");
					break;
				}
				case None<T> none:
				{
					Console.WriteLine("No value.");
					break;
				}
			}
		}

		private static Maybe<int> GetAgeInYears(Person arg)
			=> !arg.Birthdate.HasValue ?
				Maybe<int>.None() :
				Maybe<int>.Some(DateTime.Now.Year - arg.Birthdate.Value.Year);

		private static Maybe<Person> GetById(int id)
		{
			switch (id)
			{
				case 13:
					return Maybe<Person>.Some(new Person
					                          {
						                          FirstName = "Eduard",
						                          LastName = "Popescu",
						                          Birthdate = new DateTime(1982, 03, 05)
					                          });
				case 14:
					return Maybe<Person>.Some(new Person
					                          {
						                          FirstName = "John"
					                          });
				default:
					return Maybe<Person>.None();
			}
		}

		private static Maybe<string> GetFullName(Person person)
			=> person == null || string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ?
				Maybe<string>.None() :
				Maybe<string>.Some($"{person.LastName}, {person.FirstName}");
	}

	internal class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Birthdate { get; set; }
	}
}