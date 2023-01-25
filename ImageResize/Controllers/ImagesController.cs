using ImageResize.Data;
using ImageResize.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageResize.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private ImageResizeDbContext _dbContext;
        public ImagesController(ImageResizeDbContext dbContext) => _dbContext = dbContext;

        //summary: permette l'inserimento di una nuova immagine
        //params:
        //  imageIn: contiene l'immagine da caricare
        //returns:
        //  BadRequest():
        //      - se non e' stato inserito un nome o ha piu' di 100 caratteri
        //      - se non e' stata caricata un immagine
        //      - se esiste un immagine con lo stesso nome
        //  201: se l'immagine e' stata caricata con successo
        [HttpPost]
        public IActionResult UploadImage([FromForm] Image imageIn)
        {
            //verifico se e' stato inserito un nome per l'immagine e se la sua dimensione e' minore di 100
            if(imageIn.Name == null || imageIn.Name.Length == 0 || imageIn.Name.Length > 100)
                return BadRequest("Necessario inserire un nome con un numero di caratteri inferiore a 100");

            //controllo se esiste un'immagine nella richiesta
            if(imageIn.realFigure == null)
                return BadRequest("Necessario caricare un immagine");

            //verifico se il nome e' stato gia' utilizzato
            var imageDb = _dbContext.Images.Where(u => u.Name == imageIn.Name);
            if(imageDb != null)
                BadRequest("Nome gia' utilizzato");
            
            //creo un path per la memorizzazione dell'immagine in wwwroot
            var newPath = Path.Combine("wwwroot", imageIn.Name);

            //creo un nuovo file
            var fileStream = new FileStream(newPath, FileMode.Create);

            //copio l'immagine nel nuovo file
            imageIn.realFigure.CopyTo(fileStream);

            //memorizzo l'url dell'immagine senza wwwroot
            imageIn.ImageUrl = newPath.Remove(0, 7);
            
            //memorizzo l'immagine all'interno del database
            _dbContext.Images.Add(imageIn);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult ListImage()
        {
            return Ok();
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteImage([FromBody] string Name)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult ResizeImage([FromBody] string Name, int width, int Heigth)
        {
            return Ok();
        }

    }
}
