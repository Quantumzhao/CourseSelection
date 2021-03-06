﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIEngine;
using UIEngine.Core;

namespace CourseScheduler.Core.DataStrucures
{
	public class Section : IVisible
	{
		public Section(string course, string name, int openSeats, int waitList, params ClassSequence[] classTypesAndSchedules)
		{
			Course = course;
			Name = name;
			OpenSeats = openSeats;
			WaitList = waitList;

			foreach (var myClass in classTypesAndSchedules)
			{
				ClassSequences.Add(myClass);
				if (!Instructors.Contains(myClass.Instructor))
				{
					Instructors.Add(myClass.Instructor);
				}
			}
		}

		// like {LEC, class times ...}
		[Visible(nameof(ClassSequences), name: nameof(ClassSequences))]
		public List<ClassSequence> ClassSequences { get; } = new List<ClassSequence>();
		public string Course { get; }
		public string Name { get; set; }
		[Visible(nameof(OpenSeats), name: nameof(OpenSeats))]
		public int OpenSeats { get; set; }
		[Visible(nameof(WaitList), name: nameof(WaitList))]
		public int WaitList { get; set; }
		public string Description => string.Empty;
		public string Header => Name;

		[Visible(nameof(Instructors), name:nameof(Instructors))]
		public List<string> Instructors { get; } = new List<string>();

		public override string ToString() => Name;

		public bool IsOverlap(Section another)
		{
			foreach (var myClass in ClassSequences)
			{
				foreach (var anotherClass in another.ClassSequences)
				{
					if (myClass.IsOverlap(anotherClass))
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool IsAvailable(bool isOpenSectionOnly, bool doesShowFC, IEnumerable<ClassSpan> allClassSpanConstrains, IEnumerable<string> allInstructorsConstrains)
		{
			if (OpenSeats == 0 && isOpenSectionOnly)
			{
				return false;
			}

			if (Name[0] == 'F' && !doesShowFC)
			{
				return false;
			}

			if (Instructors.Intersect(allInstructorsConstrains).Count() != 0)
			{
				return false;
			}

			foreach (var @class in ClassSequences)
			{
				foreach (var weekday in @class.Weekdays)
				{
					foreach (var classSpan in weekday.ClassSpansInSingleDay)
					{
						foreach (var anotherClassSpan in allClassSpanConstrains)
						{
							if (anotherClassSpan.IsOverlap(classSpan))
							{
								return false;
							}
						}
					}
				}
			}

			return true;
		}
	}
}
