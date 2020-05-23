﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SemesterProject.ApiData.Helpers
{
	public class PagedList<T> : List<T>
	{
		public string NextPageLink { get; set; }
		public string PreviousPageLink { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public bool HasPrevious => (CurrentPage > 1);
		public bool HasNext => (CurrentPage < TotalPages);
		public PagedList()
		{

		}
		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			if (items != null)
			{
				AddRange(items);
			}
		}
		public static PagedList<T> Create(List<T> source, int pageNumber, int pageSize)
		{
			if (source != null)
			{
				var count = source.Count;
				var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
				return new PagedList<T>(items, count, pageNumber, pageSize);
			}
			else
			{
				return new PagedList<T>(null, 0, 0, 0);
			}
		}
	}
}
