using ImageResize.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageResize.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        //summary: permette l'inserimento di una nuova immagine
        //params:
        //  imageIn: contiene l'immagine da caricare
        //returns:
        //  BadRequest(): se esiste un'immagine con lo stesso nome
        //  201: se l'immagine e' stata caricata con successo
        [HttpPost]
        public IActionResult UploadImage([FromForm] Image imageIn)
        {
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
