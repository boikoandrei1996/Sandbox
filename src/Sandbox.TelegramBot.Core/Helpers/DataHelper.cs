using System.Collections.Generic;
using Sandbox.TelegramBot.Core.Enums;
using Sandbox.TelegramBot.Core.Models;

namespace Sandbox.TelegramBot.Core.Helpers
{
    internal static class DataHelper
    {
        private static readonly CategoryItemModel[] GolubikaItems = new CategoryItemModel[]
        {
            new()
            {
                Name = "Блюголд",
                Age = "6-8 месяцев",
                Price = "2.1 BYN (62 ₽)",
                DetailsUrl = "https://bluecoin.by/%D0%BA%D0%B0%D1%82%D0%B0%D0%BB%D0%BE%D0%B3-%D0%BF%D1%80%D0%BE%D0%B4%D1%83%D0%BA%D1%86%D0%B8%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/golubika.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "6-8 месяцев",
                Price = "2.1 BYN (62 ₽)",
                DetailsUrl = "https://bluecoin.by/%D0%BA%D0%B0%D1%82%D0%B0%D0%BB%D0%BE%D0%B3-%D0%BF%D1%80%D0%BE%D0%B4%D1%83%D0%BA%D1%86%D0%B8%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/golubika_main.jpg"
            },
            new()
            {
                Name = "Блюголд",
                Age = "1 год",
                Price = "3.8 BYN (112 ₽)",
                DetailsUrl = "https://bluecoin.by/%D0%BA%D0%B0%D1%82%D0%B0%D0%BB%D0%BE%D0%B3-%D0%BF%D1%80%D0%BE%D0%B4%D1%83%D0%BA%D1%86%D0%B8%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/golubika.jpg"
            }
        };

        private static readonly CategoryItemModel[] KlukvaItems = new CategoryItemModel[]
        {
            new()
            {
                Name = "Клюква 1",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%BA%D0%BB%D1%8E%D0%BA%D0%B2%D1%8B.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/klukva/klukva.jpg"
            },
            new()
            {
                Name = "Клюква 2",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%BA%D0%BB%D1%8E%D0%BA%D0%B2%D1%8B.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/klukva/klukva.jpg"
            },
            new()
            {
                Name = "Клюква 3",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%BA%D0%BB%D1%8E%D0%BA%D0%B2%D1%8B.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/klukva/klukva.jpg"
            }
        };

        private static readonly CategoryItemModel[] BrusnikaItems = new CategoryItemModel[]
        {
            new()
            {
                Name = "Брусника 1",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%B1%D1%80%D1%83%D1%81%D0%BD%D0%B8%D0%BA%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/brusnika/brusnika.jpg"
            },
            new()
            {
                Name = "Брусника 2",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%B1%D1%80%D1%83%D1%81%D0%BD%D0%B8%D0%BA%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/brusnika/brusnika.jpg"
            },
            new()
            {
                Name = "Брусника 3",
                Age = "",
                Price = "",
                DetailsUrl = "https://bluecoin.by/%D1%81%D0%B0%D0%B6%D0%B5%D0%BD%D1%86%D1%8B-%D0%B1%D1%80%D1%83%D1%81%D0%BD%D0%B8%D0%BA%D0%B8.html",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/brusnika/brusnika.jpg"
            }
        };

        public static readonly Dictionary<SurveyQuestionType, SurveyQuestionModel> SurveyQuestions = new()
        {
            {
                SurveyQuestionType.Question1,
                new()
                {
                    Text = "Какая продукция Вас интересует?",
                    Answers = new()
                    {
                        { 1, new() { Text = "голубика" } },
                        { 2, new() { Text = "брусника" } },
                        { 3, new() { Text = "клюква" } },
                        { 4, new() { Text = "декоративка" } },
                        { 5, new() { Text = "плодовые деревья и кустарники" } },
                        { Constants.CustomAnswerId, new() { Text = "свой вариант" } }
                    }
                }
            },
            {
                SurveyQuestionType.Question2,
                new()
                {
                    Text = "Для чего необходимы саженцы?",
                    Answers = new()
                    {
                        { 1, new() { Text = "на доращивание" } },
                        { 2, new() { Text = "для промышленной плантации" } },
                        { 3, new() { Text = "для увеличения ассортимента" } },
                        { 4, new() { Text = "для перепродажи" } },
                        { 5, new() { Text = "для ландшафтного дизайна" } },
                        { Constants.CustomAnswerId, new() { Text = "свой вариант" } }
                    }
                }
            }
        };

        public static readonly Dictionary<CategoryType, CategoryModel> CatalogCategories = new()
        {
            {
                CategoryType.Golubika,
                new()
                {
                    Title = "Голубика",
                    Items = GolubikaItems
                }
            },
            {
                CategoryType.Klukva,
                new()
                {
                    Title = "Клюква",
                    Items = KlukvaItems
                }
            },
            {
                CategoryType.Brusnika,
                new()
                {
                    Title = "Брусника",
                    Items = BrusnikaItems
                }
            }
        };

    }
}
