namespace DTO
{
    public class BaseDto
    {
        public string Credential { get; set; }

        public bool ShouldSerializeCredential()
        {
            return false;
        }
    }
}