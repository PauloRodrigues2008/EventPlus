using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    /// <summary>
    /// Endopoint da API que faz chamada para o método de buscar um usuário por id
    /// </summary>
    /// <param name="id"> id do usuário a ser buscado</param>
    /// <returns>Status code 200 e o usuário buscado</returns>

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(Guid id)

    {
        try
        {
            return Ok(_usuarioRepository.BuscarPorId(id));
        }
        catch (Exception error)
        {

            return BadRequest(error.Message);
        }
    
    }

    /// <summary>
    ///  Endpoint da API que faz chamada para o método de cadastrar um usuário
    /// </summary>
    /// <param name="usuario"> Usuário a ser cadastrado </param>
    /// <returns>Status code 201 e o usuário cadastrado</returns>
    

    [HttpPost]

    public IActionResult Cadastrar(Usuario usuario)
    {
        try
        {
            _usuarioRepository.Cadastrar(usuario);
            return StatusCode(201, usuario);

        }
        catch (Exception error)
        {

           return BadRequest(error.Message);
        }
    
    }

}

