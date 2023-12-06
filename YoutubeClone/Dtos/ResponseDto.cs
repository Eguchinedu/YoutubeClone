namespace YoutubeClone.Dtos
{
    public class ResponseDto
    {
        public string errorReason { get; set; }

        public bool ? success { get; set; }

        public string token { get; set; }

        public string successMessage { get; set; }
    }
}
