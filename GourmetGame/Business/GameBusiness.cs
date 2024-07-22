using GourmetGame.Data;
using GourmetGame.Models;

namespace GourmetGame.Business
{
    public class GameBusiness
    {
        public List<InsertDishesModel> ListDishes(List<KeyValuePair<string, object>> list)
        {
            var listResult = new List<InsertDishesModel>();
            foreach (var item in list)
            {
                var dish = new InsertDishesModel
                {
                    DishName =  item.Value as List<string>,
                    Hint = item.Key
                };
                listResult.Add(dish);
            }

            listResult = listResult.OrderBy(l => l.Hint).ToList();
            if (listResult.Count > 0)
            {
                var option = new InsertDishesModel
                {
                    Hint = "Nenhuma das Opcoes"
                };
                listResult.Add(option);
            }
            
            return listResult;
        }
        public InsertDishesModel GetDish(KeyValuePair<string, object> list, string dishName)
        {
            var name = new List<string>();
            var dish = new InsertDishesModel
            {

                Hint = list.Key
            };
            if (list.Value is List<string> listValue)
                foreach (var item in listValue)
                {

                    name.Add(item);
                }

            name.Add(dishName);

            dish.DishName.AddRange(name);


            return dish;
        }

        public PredictionFoodModel PredictionFoodResponse(string selectedHint)
        {
            var memoryCacheDb = MemoryCacheDb.Instance;
            var predictedFood = memoryCacheDb.TryGetValue(selectedHint, out var dish);
            var selectedDish = string.Empty;
            List<string> listPers = null;
            if (dish is List<string> list)
            {
                listPers = list;
            }
            if (memoryCacheDb.StorageSearch.Count > 0)
            {

                selectedDish = listPers.FirstOrDefault(listDish => !memoryCacheDb.StorageSearch.Contains(listDish));
                memoryCacheDb.StorageSearch.Add(selectedDish);

                if (string.IsNullOrEmpty(selectedDish))
                {
                    memoryCacheDb.StorageSearch = new List<string>();
                    return null;
                }
                else
                    return new PredictionFoodModel
                    {
                        DishName = selectedDish,
                        Hint = selectedHint
                    };                

            }

            memoryCacheDb.StorageSearch.Add(listPers.FirstOrDefault());

            return new PredictionFoodModel
            {
                DishName = listPers.FirstOrDefault(),
                Hint = selectedHint
            };


        }

        public List<InsertDishesModel> OPtionsDishesResponse()
        {
            var memoryCacheDb = MemoryCacheDb.Instance;
            var list = memoryCacheDb.GetAllItems().ToList();
            var parse = new GameBusiness();
            return parse.ListDishes(list);
        }

        public void InsertDishResponse(InsertDishesModel dish)
        {
            var memoryCacheDb = MemoryCacheDb.Instance;
            var predictedFood = memoryCacheDb.GetItem(dish.Hint);
            var parse = new GameBusiness();
            if (!string.IsNullOrEmpty(predictedFood.Key))
            {
                var list = parse.GetDish(predictedFood, dish.DishName.FirstOrDefault());
                memoryCacheDb.AddOrUpdate(dish.Hint, list.DishName);

            }
            else
                memoryCacheDb.AddOrUpdate(dish.Hint, dish.DishName);
        }
    }
}
