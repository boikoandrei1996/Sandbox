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
                Price = "2.1 BYN (61 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_0-6.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "6-8 месяцев",
                Price = "2.1 BYN (61 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_0-6.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "1 год",
                Price = "3.8 BYN (111 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_1.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "1 год",
                Price = "3.8 BYN (111 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_1.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "1.5 года",
                Price = "5.1 BYN (148 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_1-5.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "1.5 года",
                Price = "5.1 BYN (148 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_1-5.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "2 года",
                Price = "6.3 BYN (183 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_2.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "2 года",
                Price = "6.3 BYN (183 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_2.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "3 года",
                Price = "9.6 BYN (279 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_3.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "3 года",
                Price = "9.6 BYN (279 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_3.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "4 года",
                Price = "25.2 BYN (730 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_4.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "4 года",
                Price = "25.2 BYN (730 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_4.jpg"
            },

            new()
            {
                Name = "Блюголд",
                Age = "5-7 лет",
                Price = "150.9 BYN (4 370 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_5.jpg"
            },
            new()
            {
                Name = "Блюголд Инвитро",
                Age = "5-7 лет",
                Price = "150.9 BYN (4 370 ₽)",
                RedirectUrl = "https://t.me/bluecoinby_bot",
                FileUrl = "https://sandboxtgbotstorage.blob.core.windows.net/bluecoin-images/golubika/bluegold_invitro_5.jpg"
            }
        };

        private static readonly CategoryItemModel[] KlukvaItems = new CategoryItemModel[]
        {
        };

        private static readonly CategoryItemModel[] BrusnikaItems = new CategoryItemModel[]
        {
        };

        private static readonly CategoryItemModel[] ZhimolostiItems = new CategoryItemModel[]
        {
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
            },
            {
                CategoryType.Zhimolosti,
                new()
                {
                    Title = "Жимолости",
                    Items = ZhimolostiItems
                }
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
            },
            {
                SurveyQuestionType.Question3,
                new()
                {
                    Text = "Какое количество саженцев необходимо?",
                    Answers = new()
                    {
                        { 1, new() { Text = "до 100шт" } },
                        { 2, new() { Text = "100-499" } },
                        { 3, new() { Text = "500-1000" } },
                        { 4, new() { Text = "1001-4999" } },
                        { 5, new() { Text = "5000-10000" } },
                        { 6, new() { Text = "более 10000" } }
                    }
                }
            },
            {
                SurveyQuestionType.Question4,
                new()
                {
                    Text = "Куда доставить саженцы?",
                    Answers = new()
                    {
                        { 1, new() { Text = "Москва и область" } },
                        { 2, new() { Text = "Питер и область" } },
                        { 3, new() { Text = "Краснодарский край" } },
                        { 4, new() { Text = "Ярославская область" } },
                        { 5, new() { Text = "Брянская область" } },
                        { 6, new() { Text = "Нижний Новгород" } },
                        { 7, new() { Text = "Смоленская область" } },
                        { 8, new() { Text = "Казанская область" } },
                        { 9, new() { Text = "Ростовская область" } },
                        { 10, new() { Text = "Воронежская область" } },
                        { Constants.CustomAnswerId, new() { Text = "свой вариант" } }
                    }
                }
            }
        };
    }
}
