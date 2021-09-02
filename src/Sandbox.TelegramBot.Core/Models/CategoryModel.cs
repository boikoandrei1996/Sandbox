namespace Sandbox.TelegramBot.Core.Models
{
    internal class CategoryModel
    {
        public string Title { get; init; } = null!;
        public CategoryItemModel[] Items { get; init; } = null!;
    }

    internal class CategoryItemModel
    {
        public string Name { get; init; } = null!;
        public string Age { get; init; } = null!;
        public string Price { get; init; } = null!;
        public string DetailsUrl { get; init; } = null!;
        public string FileUrl { get; set; } = null!;
    }
}
