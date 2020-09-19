using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace TransliterationLibrary
{
    internal static class ConfigParser
    {
		public static OperationResult<JObject> GetTransliterationDictionary(string code)
		{
			try
			{
				var configFile = Properties.Resources.Config;
				var dictionaries = Parse(configFile);

				JToken transliterationDictionary;
				dictionaries.TryGetValue(code, out transliterationDictionary);
				if(transliterationDictionary == null || !transliterationDictionary.HasValues)
				{
					return OperationResult<JObject>.Fail(Constants.Errors.NoCode);
				}

				return OperationResult<JObject>.Success(transliterationDictionary.ToObject<JObject>());
			}
			catch (Exception ex)
			{
				return OperationResult<JObject>.Fail(ex.Message);
			}
		}

		private static JObject Parse(byte[] configFile)
		{
			using (var ms = new MemoryStream(configFile))
			{
				using (var reader = new StreamReader(ms))
				{
					var data = reader.ReadToEnd();
					return JObject.Parse(data);
				}
			}
		}
    }
}
