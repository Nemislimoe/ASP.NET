using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class CardViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string DetailsUrl { get; set; }
    public string Category { get; set; }
}

public class GalleryModel : PageModel
{
    public List<CardViewModel> Cards { get; set; }

    public void OnGet()
    {
        // Демонстраційні дані (можна замінити на базу або API)
        Cards = new List<CardViewModel>
        {
            new CardViewModel { Title = "Гора", Description = "Красива вершина з видом.", ImageUrl = "https://images.pexels.com/photos/414513/pexels-photo-414513.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", DetailsUrl = "#", Category = "Природа" },
            new CardViewModel { Title = "Місто ввечері", Description = "Нічне місто з вогнями.", ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.Awxx-n9PkbuYh2v1aKEOkQHaEK?cb=ucfimgc2&rs=1&pid=ImgDetMain&o=7&rm=3", DetailsUrl = "#", Category = "Міста" },
            new CardViewModel { Title = "Пляж", Description = "Теплий пісок і хвилі.", ImageUrl = "https://p0.pikist.com/photos/853/132/sea-ocean-water-waves-nature-white-sand-beach-shore.jpg", DetailsUrl = "#", Category = "Природа" },
            new CardViewModel { Title = "Кав\'ярня", Description = "Затишна кав\'ярня для творчості.", ImageUrl = "https://static.espreso.tv/uploads/photobank/320000_321000/320518_4_new_960x380_0.webp", DetailsUrl = "#", Category = "Життя" },
            new CardViewModel { Title = "Архітектура", Description = "Історична будівля з деталями.", ImageUrl = "https://tse2.mm.bing.net/th/id/OIP.pjwxdHTZyWi-H7KfYySl1wHaFj?cb=ucfimgc2&rs=1&pid=ImgDetMain&o=7&rm=3", DetailsUrl = "#", Category = "Міста" },
            new CardViewModel { Title = "Лісова стежка", Description = "Спокійна прогулянка серед дерев.", ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.n7_pdODQI4wzfsqtSqfW9gHaEq?cb=ucfimgc2&rs=1&pid=ImgDetMain&o=7&rm=3", DetailsUrl = "#", Category = "Природа" },
            new CardViewModel { Title = "Сучасне мистецтво", Description = "Кольори та форми.", ImageUrl = "https://tse2.mm.bing.net/th/id/OIP.-KoX69CtmJPgn9CHqOIorAHaFF?cb=ucfimgc2&rs=1&pid=ImgDetMain&o=7&rm=3", DetailsUrl = "#", Category = "Мистецтво" },
            new CardViewModel { Title = "Захід сонця", Description = "Теплі відтінки на горизонті.", ImageUrl = "https://th.bing.com/th/id/R.112fabc54f8f252d7cd0391a9f37e41d?rik=cuGOBNr5RgwrwA&pid=ImgRaw&r=0", DetailsUrl = "#", Category = "Природа" },
        };
    }
}
