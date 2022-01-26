namespace FedjazContest.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Base64 { get; set; } = "";

        public Image(string base64)
        {
            Base64 = base64;
        }
    }
}
