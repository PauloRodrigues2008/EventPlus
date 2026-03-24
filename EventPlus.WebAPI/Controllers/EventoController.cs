using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventoController : ControllerBase
{
    private readonly IEventoRepository _eventoRepository;

    public EventoController(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }


    /// <summary>
    /// Endpoint da API que faz chamada para o método de listar os eventos filtrando pela id do usuário
    /// </summary>
    /// <param name="IdUsuario">Id do usuário para filtragem</param>
    /// <returns>Status code 200 e um listar de eventos</returns>
    [HttpGet("Usuario/{IdUsuario}")]
    public IActionResult ListarPorId(Guid IdUsuario)
    {
        try
        {
            return Ok(_eventoRepository.ListarPorId(IdUsuario));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz chamada para o método de listar.
    /// </summary>
    /// <returns>Status code 200 e a lista dos próximos eventos</returns>
    [HttpGet("ListarProximos")]
    public IActionResult BuscarProximosEventos()
    {
        try
        {
            return Ok(_eventoRepository.ProximosEventos());
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet]
    public IActionResult Listar()
    {
        try
        {
            return Ok(_eventoRepository.Listar());
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }
    [HttpPost]
    public IActionResult Cadastrar(EventoDTO Evento)
    {
        try
        {
            var novoEvento = new Evento
            {
                Nome = Evento.Nome!,
                DataEvento = Evento.DataEvento!,
                Descricao = Evento.Descricao!,
                IdTipoEvento = Evento.IdTipoEvento!,
                IdInstituicao = Evento.IdInstituicao!
            };

            _eventoRepository.Cadastrar(novoEvento);
            return StatusCode(201, novoEvento);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }
    [HttpPut("{id}")]
    public IActionResult Atualizar(Guid id, EventoDTO evento)
    {
        try
        {
            var EventoAtualizado = new Evento
            {
                Nome = evento.Nome!,
                DataEvento = evento.DataEvento!,
                Descricao = evento.Descricao!,
                IdTipoEvento = evento.IdTipoEvento!,
                IdInstituicao = evento.IdInstituicao!
            };

            _eventoRepository.Atualizar(id, EventoAtualizado);
            return StatusCode(204, EventoAtualizado);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz chamada para o método de deletar o tipo de Evento
    /// </summary>
    /// <param name="id">Tipo de Evento a ser excluido</param>
    /// <returns>Status code 204</returns>

    [HttpDelete("{id}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            _eventoRepository.Deletar(id);
            return NoContent();  // Retorna status code 204 para indicar que a operação foi bem-sucedida, mas não há conteúdo para retornar
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


}