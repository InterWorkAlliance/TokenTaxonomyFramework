using System;
using Microsoft.Extensions.Caching.Memory;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy
{
	
	internal static class TaxonomyCache
	{
		private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
		
		public static void SaveToCache(string cacheKey, Model.Taxonomy taxonomy, DateTime absoluteExpiration)
		{
			_cache.Set(cacheKey.ToLower(), taxonomy, absoluteExpiration);
		}

		public static Model.Taxonomy GetFromCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) as Model.Taxonomy;
		}

		public static void RemoveFromCache(string cacheKey)
		{
			_cache.Remove(cacheKey.ToLower());
		}

		public static bool IsInCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) != null;
		}
	}

	internal static class TokenTemplateCache
	{
		private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
		
		public static void LoadTemplates(TokenTemplates templates)
		{
			foreach (var t in templates.Template)
			{
				SaveToCache(t.Key, t.Value, DateTime.Now.AddDays(1));
			}
		}
		public static void SaveToCache(string cacheKey, TokenTemplate template, DateTime absoluteExpiration)
		{
			_cache.Set(cacheKey.ToLower(), template, absoluteExpiration);
		}

		public static TokenTemplate GetFromCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) as TokenTemplate;
		}

		internal static void ResetCache()
		{
			_cache.Dispose();
			_cache = new MemoryCache(new MemoryCacheOptions());

		}
		public static void RemoveFromCache(string cacheKey)
		{
			_cache.Remove(cacheKey.ToLower());
		}

		public static bool IsInCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) != null;
		}
	}
}