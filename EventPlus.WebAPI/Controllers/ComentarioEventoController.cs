using Azure;
using Azure.AI.ContentSafety;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioEventoController : ControllerBase
{
    private readonly ContentSafetyClient _ContentSafetyClient;

    private readonly IComentarioEventoRepository _comentarioEventoRepository;
    public ComentarioEventoController(ContentSafetyClient contentSafetyClient, IComentarioEventoRepository comentarioEventoRepository)
    {
        _ContentSafetyClient = contentSafetyClient;
        _comentarioEventoRepository = comentarioEventoRepository;
   
    }


    [HttpGet("{IdEvento}/{IdUsuario}")]
    public IActionResult BuscarPorIdUsuario(Guid IdUsuario, Guid IdEvento)
    {
        try
        {
            var comentario = _comentarioEventoRepository.BuscarPorIdUsuario(IdUsuario, IdEvento);
            if (comentario != null)
                return NotFound();
            return Ok(comentario);
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }




    /// <summary>
    /// Endpoint da API que cadastra a modera um comentario
    /// </summary>
    /// <param name="comentarioEvento">comentário a ser moderado</param>
    /// <returns>Status Code 201 e o comentário criado </returns>
    [HttpPost]
   public async Task<IActionResult> Cadastrar(ComentarioEventoDTO comentarioEvento)
    {
        try
        {
            if (string.IsNullOrEmpty(comentarioEvento.Descricao))
            {
            
                return BadRequest("A descrição do comentário é obrigatória.");
            }

            //criar objeto de análise 
            var request = new AnalyzeTextOptions(comentarioEvento.Descricao);

            //chamar a API do Azure Content Safety
            Response<AnalyzeTextResult> response = await
                _ContentSafetyClient.AnalyzeTextAsync(request); 

            // Verificar se o texto tem alguma severidade maior que 0
            bool temConteudoInapropriado = response.Value.CategoriesAnalysis.Any
                (comentario => comentario.Severity > 0);

            var novoComentario = new ComentarioEvento
            {
               
                Descricao = comentarioEvento.Descricao,
                IdUsuario = comentarioEvento.IdUsuario,
                IdEvento = comentarioEvento.IdEvento,
                DataComentarioEvento = DateTime.Now,
                //Defeni se o comentário vai ser exibido
                Exibe = !temConteudoInapropriado

            };  

            // cadastrar o comentario
            _comentarioEventoRepository.Cadastrar(novoComentario);  
            return StatusCode(201, novoComentario);

        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet]

    public IActionResult Listar(Guid IdEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.Listar(IdEvento));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }

    }




    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _comentarioEventoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }
}
