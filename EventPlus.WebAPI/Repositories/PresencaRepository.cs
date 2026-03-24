using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

    public class PresencaRepository: IPresencaRepository
    {
    private readonly EventContext _eventContext;

    public PresencaRepository(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public void Atualizar(Guid id, Presenca IdPresencaEvento)
    {
        var presencaBuscado = _eventContext.Presencas.Find(IdPresencaEvento);
        if (presencaBuscado != null)
        {
            presencaBuscado.Situacao = !presencaBuscado.Situacao;


            _eventContext.SaveChanges();
        }
    }



    /// <summary>
    /// Busca uma presenca por seu id 
    /// </summary>
    /// <param name="id">id da presença a ser buscado</param>
    /// <returns>preesença buscada</returns>
    public Presenca BuscarPorId(Guid id)
    {
        return _eventContext.Presencas
            .Include(p => p.IdEventoNavigation)
            .ThenInclude(e => e!.IdInstituicaoNavigation)
            .FirstOrDefault(p => p.IdPresenca == id)!;
    }

    public void Deletar(Guid id)
    {
       var presencaBuscado = _eventContext.Presencas.Find(id);
        if (presencaBuscado != null)
        {
            _eventContext.Presencas.Remove(presencaBuscado);
            _eventContext.SaveChanges();
        }   
    }

    public void Inscrever(Presenca Incricao)
    {
        _eventContext.Presencas.Add(Incricao);
        _eventContext.SaveChanges();
    }

    public List<Presenca> Listar()
    {
        return _eventContext.Presencas
            .OrderBy(p => p.IdPresenca) 
            .ToList();


    }
    /// <summary>
    /// Lista as presenças de um usuário es específico
    /// </summary>
    /// <param name="IdUsuario">id do usuário para </param>
    /// <returns>uma lista de presença de uma usuario específico</returns>
    public List<Presenca> ListarMinhas(Guid IdUsuario)
    {
        return _eventContext.Presencas
            .Include(p => p.IdEventoNavigation)
            .ThenInclude(e => e!.IdInstituicaoNavigation)
            .Where(p => p.IdUsuario == IdUsuario)
            .ToList();                  
    }
}

