namespace GourmetGame.Models
{
    public class InsertDishesModel
    {
        public List<string> DishName { get; set; } = new List<string>();
        public string? Hint { get; set; }
    }
}
