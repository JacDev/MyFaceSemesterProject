using System;
using System.Collections.Generic;
using System.Text;

namespace SemesterProject.ApiData.Helpers
{
	public class PaginationMetadata
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public string NextPageLink { get; set; }
		public string PreviousPageLink { get; set; }
	}
}
