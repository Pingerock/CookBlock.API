namespace CookBlock.API.Models
{
    public class Recipe_Instruction
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int Position { get; set; }
        public string? Picture_base64 { get; set; }
        public string Text { get; set; }
    }
}
