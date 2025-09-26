using MedViaApi.Models;
using Mscc.GenerativeAI;
using System.Text.Json;

namespace MedViaApi.Services
{
    public class MedService
    {
        private readonly IGenerativeAI _googleAI;

        public MedService(IGenerativeAI googleAI)
        {
            _googleAI = googleAI;
        }

            public async Task<MedicineRespone?>GetMedicineDetails(string MedcineName)
        {
            var prompt = $@"
                        You are a medical AI.  
                        Return ONLY a valid JSON object with the following keys: useCase, targetAudience, dosageRecommendations, sideEffects, precautions.  

                        Write each value in **simple, easy-to-understand language**, suitable for anyone including elderly people.  
                        Use short sentences, avoid medical jargon, and be clear and friendly.  

                        Medicine: {MedcineName}";


            var model = _googleAI.GenerativeModel(model: Model.Gemini25Flash);

            var response = await model.GenerateContent(prompt);



            var json = response.Text.Trim();

            if (json.StartsWith("```"))
            {
                var start = json.IndexOf('{');
                var end = json.LastIndexOf('}');
                if (start >= 0 && end > start)
                {
                    json = json.Substring(start, end - start + 1);
                }
            }



            try
            {
                return JsonSerializer.Deserialize<MedicineRespone>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parse Error: {ex.Message}\nResponse was:\n{json}");
                return null;
            }

        }

    }
}
