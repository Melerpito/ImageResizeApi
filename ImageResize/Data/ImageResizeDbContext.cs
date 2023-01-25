using ImageResize.Models;

//contiene la classe DbContext, permette il code first approach
using Microsoft.EntityFrameworkCore;

namespace ImageResize.Data
{
    public class ImageResizeDbContext : DbContext
    {
        //costruttore che richiama il costrutto di DbContext,
        //permette la realizzazione di un database basato sulle classi in Models
        public ImageResizeDbContext(DbContextOptions<ImageResizeDbContext> options) : base(options) { }

        //tabella contenete i metadati delle immagini memorizzate
        public DbSet<Figure> Figures { get; set; }
    }
}
