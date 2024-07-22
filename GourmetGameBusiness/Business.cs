using GourmetGame.Business;
using GourmetGame.Data;
using GourmetGame.Models;

namespace BusinessTests
{
    public class Business
    {
        [Theory]
        [InlineData("Hint1", "Dish1")]
        [InlineData("Hint2", "Dish2")]
        public void ListDishes_ShouldReturnCorrectList(string hint, string dishName)
        {
            // Arrange
            var parseData = new GameBusiness();
            var list = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(hint, new List<string> { dishName })
            };

            // Act
            var result = parseData.ListDishes(list);
            var dishNameList = new List<string>();
            foreach (var item in result)
            {
                foreach (var dish in item.DishName.ToList())
                {
                    dishNameList.Add(dish);
                }
            }
            // Assert
            Assert.NotNull(result);
            Assert.Contains(result, dish => dish.Hint == hint);
            Assert.Contains(dishNameList, dish => dish.Contains(dishName));
        }

        [Theory]
        [InlineData("Hint1", "Dish1", "Dish3")]
        [InlineData("Hint2", "Dish2", "Dish4")]
        public void GetDish_ShouldReturnCorrectDish(string hint, string existingDishName, string newDishName)
        {
            // Arrange
            var parseData = new GameBusiness();
            var list = new KeyValuePair<string, object>(hint, new List<string> { existingDishName });

            // Act
            var result = parseData.GetDish(list, newDishName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hint, result.Hint);
            Assert.Contains(existingDishName, result.DishName);
            Assert.Contains(newDishName, result.DishName);
        }

        [Theory]
        [InlineData("Hint1", "Dish1", "Dish2")]
        public void PredictionFoodResponse_ShouldReturnCorrectModel(string hint, string dish1, string dish2)
        {
            // Arrange
            var parseData = new GameBusiness();
            var memoryCacheDb = MemoryCacheDb.Instance;
            memoryCacheDb.AddOrUpdate(hint, new List<string> { dish1, dish2 });

            // Act
            var result = parseData.PredictionFoodResponse(hint);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dish1, result.DishName); // First dish
            Assert.Equal(hint, result.Hint);
        }

        [Fact]
        public void OPtionsDishesResponse_ShouldReturnCorrectOptions()
        {
            // Arrange
            var parseData = new GameBusiness();
            var memoryCacheDb = MemoryCacheDb.Instance;
            memoryCacheDb.AddOrUpdate("Hint1", new List<string> { "Dish1" });
            memoryCacheDb.AddOrUpdate("Hint2", new List<string> { "Dish2" });

            // Act
            var result = parseData.OPtionsDishesResponse();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count); // Includes "Nenhuma das Opcoes"
            Assert.Contains(result, dish => dish.Hint == "Hint1");
            Assert.Contains(result, dish => dish.Hint == "Hint2");
            Assert.Contains(result, dish => dish.Hint == "Nenhuma das Opcoes");
        }

        [Theory]
        [InlineData("Hint1", "Dish1", "Dish2")]
        public void InsertDishResponse_ShouldAddDishCorrectly(string hint, string existingDishName, string newDishName)
        {
            // Arrange
            var parseData = new GameBusiness();
            var memoryCacheDb = MemoryCacheDb.Instance;
            memoryCacheDb.AddOrUpdate(hint, new List<string> { existingDishName });

            var newDish = new InsertDishesModel
            {
                DishName = new List<string> { newDishName },
                Hint = hint
            };

            // Act
            parseData.InsertDishResponse(newDish);

            // Assert
            var result = memoryCacheDb.GetItem(hint);
            Assert.NotNull(result.Value);
            var dishes = result.Value as List<string>;
            Assert.Contains(existingDishName, dishes);
            Assert.Contains(newDishName, dishes);
        }
    }
}
