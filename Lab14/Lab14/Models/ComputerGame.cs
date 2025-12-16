namespace Lab14.Models
{
    public class ComputerGame
    {
        public int Id { get; set; }            // ціле число
        public string Name { get; set; }      // рядок
        public decimal Price { get; set; }    // десятковий тип
        public DateTime ReleaseDate { get; set; } // додаткова властивість типу DateTime
    }
}
