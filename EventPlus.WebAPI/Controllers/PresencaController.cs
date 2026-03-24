using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PresencaController : ControllerBase
{
    private IPresencaRepository _presencaRepository;

    public PresencaController(IPresencaRepository presencaRepository)
    {
        _presencaRepository = presencaRepository;

    }
    [HttpGet("{id}")]


    /// <summary>
    /// Endopoint para buscar uma presença por seu id
    /// </summary>
    /// <param name="id">id da presença a ser buscada</param>
    /// <returns>Status code 200 e a presença buscada</returns>
    public IActionResult BuscarPorId(Guid id)
    {
        try
        {
            return Ok(_presencaRepository.BuscarPorId(id));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }


    }


    /// <summary>
    /// Endpoint da API que retorna um lista dde presenças filtrada por usuário
    /// </summary>
    /// <param name="idUsuario">id do usuário para filtragem</param>
    /// <returns>lista de presença filtrad pelo usuário</returns>
    [HttpGet("ListarMinhas{idUsuario}")]
    public IActionResult BuscarPorUsuario(Guid idUsuario)
    {
        try
        {
            return Ok(_presencaRepository.ListarMinhas(idUsuario));
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    
    
    }

    [HttpPost]

    public IActionResult Inscrever(PresencaDTO presenca)
    {
        try
        {
            var novoPresenca = new Presenca
            {
                IdEvento = presenca.IdEvento!,
                IdUsuario = presenca.IdUsuario!,
                Situacao = presenca.Situacao!
            };
            _presencaRepository.Inscrever(novoPresenca);
            return StatusCode(201, novoPresenca);
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);        
        }

    }
    [HttpPut("{id}")]

    public IActionResult Atualizar(Guid id, PresencaDTO presenca)
    {
        try
        {
            var PresencaAtulizado = new Presenca
            {
               IdEvento = presenca.IdEvento,
                IdUsuario = presenca.IdUsuario,
                Situacao = presenca.Situacao
            };      
           _presencaRepository.Atualizar(id, PresencaAtulizado);    
            return StatusCode(204, PresencaAtulizado);
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);    
        }
    }
    [HttpDelete("{id}")]

    public IActionResult Deletar(Guid id)
    {
        try
        {
            _presencaRepository.Deletar(id);
            return StatusCode(204);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);    
        }
    }                   
}   

