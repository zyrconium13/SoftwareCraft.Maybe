﻿using System;
using System.Linq;
using SoftwareCraft.Maybe;

namespace Sandbox
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var mPerson = GetById(13);

			if (mPerson.HasValue) Console.WriteLine("Huston, we have a person!");

			var fullName = mPerson.Bind(GetFullName);
			var age = mPerson.Bind(GetAgeInYears);

			fullName.Map(Console.WriteLine, () => Console.WriteLine("Unknown name."));
			age.Map(Console.WriteLine, () => Console.WriteLine("Unknown age."));
		}

		private static Maybe<int> GetAgeInYears(Person arg)
			=> !arg.Birthdate.HasValue ?
				new Maybe<int>() :
				new Maybe<int>(DateTime.Now.Year - arg.Birthdate.Value.Year);

		private static Maybe<Person> GetById(int id)
		{
			switch (id)
			{
				case 13:
					return new Maybe<Person>(new Person
					                         {
						                         FirstName = "Eduard",
						                         LastName = "Popescu",
						                         Birthdate = new DateTime(1982, 03, 05)
					                         });
				case 14:
					return new Maybe<Person>(new Person
					                         {
						                         FirstName = "John"
					                         });
				default:
					return new Maybe<Person>();
			}
		}

		private static Maybe<string> GetFullName(Person person)
			=> person == null || string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName) ?
				new Maybe<string>() :
				new Maybe<string>($"{person.LastName}, {person.FirstName}");
	}

	internal class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Birthdate { get; set; }
	}
}