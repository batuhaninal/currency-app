namespace Application.Models.RequestParameters.AIs
{
    public class OllamaRequest
    {
        public string Model { get; set; } = "phi3:mini";
        public string Prompt { get; set; } = string.Empty;
        public bool Stream { get; set; } = false;
        public OllamaOptions Options { get; set; } = new();
    }

    public class OllamaOptions
    {
        public double Temperature { get; set; } = 0;
    }

    public class OllamaResponse
    {
        public string Response { get; set; } = string.Empty;
    }

}