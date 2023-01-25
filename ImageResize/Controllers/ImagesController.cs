using ImageResize.Data;
using ImageResize.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
        public IActionResult UploadImage([FromForm] Figure imageIn)
        {
            //verifico se e' stato inserito un nome per l'immagine e se la sua dimensione e' minore di 100
            if(imageIn.Name == null || imageIn.Name.Length == 0 || imageIn.Name.Length > 100)
                return BadRequest("Necessario inserire un nome con un numero di caratteri inferiore a 100");

            //controllo se esiste un'immagine nella richiesta
            if(imageIn.realFigure == null)
                return BadRequest("Necessario caricare un immagine");

            //verifico se il nome e' stato gia' utilizzato
            var imageDb = _dbContext.Figures.Where(u => u.Name == imageIn.Name).SingleOrDefault();
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
            _dbContext.Figures.Add(imageIn);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        //summary:  restituisce i nomi delle immagini memorizzate
        //              in ordine alfabetico
        //returns:
        //          - NotFound() se non esistono immagini;
        //          - Ok(imagesList) altrimenti.
        [HttpGet]
        public IActionResult ListImages()
        {
            //crea la lista delle immagini da restituire
            var imagesList =
            (
                from images in _dbContext.Figures
                orderby images.Name
                select images.Name
            );

            //controllo se esistono immagini
            if(imagesList == null)
                return NotFound("Nessuna immagine");

            return Ok(imagesList);
        }

        //summary: Elimina una immagine relativa a name
        //params:
        //          - name: Contiene il nome dell'immagine da eliminare
        //returns:
        //          - BadRequest():   Se name e' null
        //          - NotFound():     Se l'immagine non e' stata trovata
        //          - Ok():           Se l'immagine e' stata eliminata
        [HttpDelete("{name}")]
        public IActionResult DeleteImage([FromBody] string name)
        {
            //controllo se e' stato inserito un nome
            if(name == null)
                return BadRequest("Necessario inserire il nome dell'immagine da eliminare");

            //controllo se il nome esiste
            var imageDb = _dbContext.Figures.Where(u => u.Name == name).SingleOrDefault();
            if (imageDb == null || imageDb.ImageUrl == null)
                NotFound("Immagine non trovata");

            //elimino l'immagine contenuta in wwwroot
            var imagePath = Path.Combine("wwwroot", imageDb.ImageUrl);
            if(System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            //Rimuoi i metadata memorizzati nel database
            _dbContext.Figures.Remove(imageDb);
            _dbContext.SaveChanges();

            return Ok("Immagine rimossa correttamente");
        }

        [HttpPut("{id}")]
        public IActionResult ResizeImage([FromBody] string name, int width, int heigth)
        {
            return Ok();
        }

    }
}
