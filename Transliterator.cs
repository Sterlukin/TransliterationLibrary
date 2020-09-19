using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace TransliterationLibrary
{
    public static class Transliterator
    {
		public static OperationResult<string> GetTransliteratedWord(string code, string word)
		{
			try
			{
				var parseResult = ConfigParser.GetTransliterationDictionary(code);
				if (!parseResult.IsSuccess)
				{
					return OperationResult<string>.Fail(parseResult.Message);
				}

				var transliterationDictionary = parseResult.Data;
				var transliteratedWord = TransliterateWord(transliterationDictionary, word);

				return OperationResult<string>.Success(transliteratedWord);
			}
			catch(Exception ex)
			{
				return OperationResult<string>.Fail(ex.Message);
			}
		}

		private static string TransliterateWord(JObject transliterationDictionary, string word)
		{
			var transliteratedWord = new StringBuilder();
			foreach(var letter in word)
			{
				var transliteratedLetter = TryTransliterateLetter(transliterationDictionary, letter);
				transliteratedWord.Append(transliteratedLetter);
			}

			return transliteratedWord.ToString();
		}

		private static string TryTransliterateLetter(JObject transliterationDictionary, char letter)
		{
			if (char.IsDigit(letter))
			{
				return letter.ToString();
			}

			if (char.IsUpper(letter))
			{
				var letterAsInDictionary = char.ToLower(letter);
				return
					TransliterateLetter(transliterationDictionary, letterAsInDictionary).
					ToString().ToUpper();
			}

			return TransliterateLetter(transliterationDictionary, letter);
		}

		private static string TransliterateLetter(JObject transliterationDictionary, char letter)
		{
			JToken transliteratedLetter;
			if (transliterationDictionary.TryGetValue(letter.ToString(), out transliteratedLetter))
			{
				return transliteratedLetter.ToString();
			}

			return Constants.UnrecognizedLetterSubstitute;
		}
    }
}
