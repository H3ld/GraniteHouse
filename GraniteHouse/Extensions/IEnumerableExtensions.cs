using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GraniteHouse.Extensions
{
	//public class SelectListItem 
	//{
	//	public string Text { get; set; }
	//	public string Value { get; set; }
	//	public bool Selected { get; set; }
	//}

	public static class IEnumerableExtensions
	{
		public static IEnumerable<SelectListItem> ToSelectListItem<T> (this IEnumerable<T> items, int selectedValue)
		{
			return from item in items
				   select new SelectListItem
				   {
					   Text = item.GetPropertyValue("Name"),
					   Value = item.GetPropertyValue("Id"),
					   Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
				   };
		}
	}
}
